using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.DataConfigs;
using Meta.Entities;
using Meta.UseCases;
using Unity.Services.CloudCode;
using Unity.Services.CloudCode.GeneratedBindings;
using UnityEngine;

namespace Meta.Gateways
{
    public class UgsHangarGateway : IHangarGateway
    {
        public event Action<long> OnSoftChanged;
        public event Action<long> OnHardChanged;
        
        private readonly IAuthenticatorService _authenticatorService;
        
        private readonly PlayerDataServiceBindings _playerDataServiceBindings;
        private readonly PlayerEconomyServiceBindings _playerEconomyServiceBindings;

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        private readonly ProfileDataConfig _profileDataConfig;
        private readonly VehiclesDataConfig _vehiclesDataConfig;
        private readonly WheelsDataConfig _wheelsDataConfig;
        
        private readonly Storage _storage;

        public async UniTask PlayWithUgs()
        {
            var playerName = "Etishka";
            Debug.Log($"send \"{playerName}\" player name");
            var response = await _playerDataServiceBindings.SayHello(playerName);
            Debug.Log($"response is: \"{response}\"");
            await SaveNewPlayerName(playerName);
        }

        private async UniTask SaveNewPlayerName(string newPlayerName)
        {
            try
            {
                var playerName = await _playerDataServiceBindings.HandleNewPlayerNameEntry(newPlayerName);
                Debug.Log($"Saved new  player name in the could: {playerName}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public UgsHangarGateway(IAuthenticatorService authenticatorService, ProfileDataConfig profileDataConfig, VehiclesDataConfig vehiclesDataConfig, WheelsDataConfig wheelsDataConfig)
        {
            _authenticatorService = authenticatorService;
            _profileDataConfig = profileDataConfig;
            _vehiclesDataConfig = vehiclesDataConfig;
            _wheelsDataConfig = wheelsDataConfig;

            if(!_authenticatorService.IsAuthenticated)
            {
                Debug.LogError("player is not authenticated");
                return;
            }

            _playerDataServiceBindings = new PlayerDataServiceBindings(CloudCodeService.Instance);
            _playerEconomyServiceBindings = new PlayerEconomyServiceBindings(CloudCodeService.Instance);
            
            PlayWithUgs().Forget();
            
            // ...
            
            _storage = new Storage
            {
                Wallet = new Wallet(),
                AllVehicles = new List<Vehicle>(),
                BoughtVehicles = new List<Vehicle>()
            };

            foreach (var vehicleData in _vehiclesDataConfig.vehicles)
            {
                var vehicle = new Vehicle
                {
                    Id = vehicleData.id,
                    AllWheels = new List<Wheels>(),
                    BoughtWheels = new List<Wheels>(),
                    BuyPrice = vehicleData.price,
                };
                
                foreach (var wheelsData in wheelsDataConfig.wheels)
                {
                    if(wheelsData.vehicleId != vehicle.Id)
                        continue;
                    
                    var wheels = new Wheels
                    {
                        Id = wheelsData.id,
                        Price = wheelsData.price,
                    };
                    
                    vehicle.AllWheels.Add(wheels);

                    if (wheels.Price == 0)
                        vehicle.StockWheels = wheels;
                }

                _storage.AllVehicles.Add(vehicle);

                var profileVehicleData = _profileDataConfig.vehiclesData.FirstOrDefault(x => x.id == vehicle.Id);

                if(profileVehicleData == null)
                    continue;

                _storage.BoughtVehicles.Add(vehicle);
                
                if(vehicle.Id == _profileDataConfig.setVehicleId)
                    _storage.CurrentVehicle = vehicle;

                foreach (var wheelsData in _profileDataConfig.wheelsData)
                {
                    var firstOrDefault = _wheelsDataConfig.wheels.FirstOrDefault(x => wheelsData.id == x.id);
                    if (firstOrDefault == null || firstOrDefault.vehicleId != vehicle.Id)
                        continue;
                    var wheels = vehicle.AllWheels.Find(x =>  x.Id == wheelsData.id);
                    if (wheels == null)
                    {
                        Debug.LogError($"wheels not found: {wheelsData.id}");
                        continue;
                    }
                    vehicle.BoughtWheels.Add(wheels);
                }

                vehicle.CurrentWheels = vehicle.AllWheels.FirstOrDefault(x =>  x.Id == profileVehicleData.currentWheelsId);
            }

            _storage.CurrentVehicle ??= _storage.BoughtVehicles.First();
        }
        
        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _playerEconomyServiceBindings.GetPlayerHard();
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await UniTask.FromResult(0);
        }


        #region Vehicles
        public async UniTask<List<Vehicle>> GetAllVehicles(CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            return _storage.AllVehicles;
        }

        public async UniTask<List<Vehicle>> GetBoughtVehicles()
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            return _storage.BoughtVehicles;
        }

        public async UniTask<Vehicle> GetSetVehicle(CancellationToken cancellationToken)
        {
            await AwaitableDummy(cancellationToken);
            return _storage.CurrentVehicle;
        }

        public async UniTask<bool> SetSetVehicle(Vehicle vehicle)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            _storage.CurrentVehicle = vehicle;
            _profileDataConfig.setVehicleId = vehicle.Id;
            return true;
        }

        public async UniTask<bool> BuyVehicle(Vehicle vehicle)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            if (_storage.BoughtVehicles.Contains(vehicle))
                return false;
            
            if (vehicle.BuyPrice > _storage.Wallet.Soft)
                return false;
            
            _storage.BoughtVehicles.Add(vehicle);
            return true;
        }
        #endregion Vehicles

        #region Wheels
        public async UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            return vehicle.AllWheels;
        }

        public async UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            return vehicle.BoughtWheels;
        }

        public async UniTask<Wheels> GetSetWheels(Vehicle vehicle, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            if(vehicle == null)
                throw new Exception("vehicle is null");
            return vehicle.CurrentWheels;
        }

        public async UniTask<bool> SetWheels(Vehicle vehicle, Wheels wheels, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            if (!vehicle.AllWheels.Contains(wheels))
                return false;

            vehicle.CurrentWheels = wheels;
            for (var index = 0; index < _profileDataConfig.vehiclesData.Count; index++)
            {
                var vehicleData = _profileDataConfig.vehiclesData[index];
                if (vehicleData.id != vehicle.Id)
                    continue;

                vehicleData.currentWheelsId = wheels.Id;
                return true;
            }

            throw new Exception($"vehicle not found: {vehicle.Id}");
        }

        public async UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheels, CancellationToken cancellationToken)
        {
            await AwaitableDummy(cancellationToken);
            if (!vehicle.AllWheels.Contains(wheels))
                return false;

            if (wheels.Price > _storage.Wallet.Soft)
            {
                Debug.Log($"not enough money for wheels: {wheels.Id}");
                return false;
            }

            _storage.Wallet.Soft -= wheels.Price;
            _profileDataConfig.soft = _storage.Wallet.Soft;
            OnSoftChanged.Invoke(_storage.Wallet.Soft);

            vehicle.CurrentWheels = wheels;
            for (var index = 0; index < _profileDataConfig.vehiclesData.Count; index++)
            {
                var vehicleData = _profileDataConfig.vehiclesData[index];
                if (vehicleData.id != vehicle.Id)
                    continue;

                vehicleData.currentWheelsId = wheels.Id;
            }
            
            vehicle.BoughtWheels.Add(wheels);
            _profileDataConfig.wheelsData.Add(new ProfileDataConfig.WheelsData
            {
                id = wheels.Id
            });

            return true;
        }
        #endregion Wheels

        public void Dispose()
        {
            // TODO release managed resources here
        }
        
        private static UniTask AwaitableDummy(CancellationToken cancellationToken)
        {
            return UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  cancellationToken);
        }
    }
}