using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.DataConfigs;
using Meta.Entities;
using Meta.UseCases;
using UnityEngine;
using UnityEngine.Scripting;

namespace Meta.Gateways
{
    [Preserve]
    public class LocalHangarGateway : IHangarGateway, IDisposable
    {
        private readonly ProfileDataConfig _profileDataConfig;
        private readonly VehiclesDataConfig _vehiclesDataConfig;
        private readonly WheelsDataConfig _wheelsDataConfig;
        
        private readonly Storage _storage;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public LocalHangarGateway(
            ProfileDataConfig profileDataConfigDataConfig,
            VehiclesDataConfig vehiclesDataConfig,
            WheelsDataConfig wheelsDataConfig)
        {
            _profileDataConfig = profileDataConfigDataConfig;
            _vehiclesDataConfig = vehiclesDataConfig;
            _wheelsDataConfig = wheelsDataConfig;
            _cancellationTokenSource = new CancellationTokenSource();

            _storage = new Storage
            {
                Wallet = new Wallet
                {
                    Hard = _profileDataConfig.hard,
                    Soft = _profileDataConfig.soft
                },
                AllVehicles = new List<Vehicle>(),
                BoughtVehicles = new List<Vehicle>()
            };

            foreach (var vehicleData in vehiclesDataConfig.vehicles)
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
                
                if(vehicle.Id == _profileDataConfig.currentVehicleId)
                    _storage.CurrentVehicle = vehicle;

                foreach (var wheelsData in _profileDataConfig.wheelsData)
                {
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
        }
        
        #region Money

        public event Action<long> OnSoftChanged = delegate { };
        public event Action<long> OnHardChanged = delegate { };

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            await AwaitableDummy(cancellationToken);
            return _storage.Wallet.Hard = _profileDataConfig.hard;
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            await AwaitableDummy(cancellationToken);
            return _storage.Wallet.Soft = _profileDataConfig.soft;
        }
        #endregion

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

        public async UniTask<Vehicle> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            await AwaitableDummy(cancellationToken);
            return _storage.CurrentVehicle;
        }

        public async UniTask<bool> SetCurrentVehicle(Vehicle vehicle)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            _storage.CurrentVehicle = vehicle;
            _profileDataConfig.currentVehicleId = vehicle.Id;
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
        public async UniTask<List<Wheels>> GetAllWheels(string vehicleId, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            var vehicle = _storage.AllVehicles.First(x => x.Id == vehicleId);
            if(vehicle == null)
                throw new Exception($"vehicle not found: {vehicleId}");
            return vehicle.AllWheels;
        }

        public async UniTask<List<Wheels>> GetBoughtWheels(string vehicleId, CancellationToken cancellationToken)
        {
            await AwaitableDummy(_cancellationTokenSource.Token);
            var vehicle = _storage.AllVehicles.First(x => x.Id == vehicleId);
            if(vehicle == null)
                throw new Exception($"vehicle not found: {vehicleId}");
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
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private UniTask AwaitableDummy(CancellationToken cancellationToken)
        {
            return UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  cancellationToken);
        }
    }
}
