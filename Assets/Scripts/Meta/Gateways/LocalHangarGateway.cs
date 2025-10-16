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
            ProfileDataConfig profileDataConfig,
            VehiclesDataConfig vehiclesDataConfig,
            WheelsDataConfig wheelsDataConfig)
        {
            _profileDataConfig = profileDataConfig;
            _vehiclesDataConfig = vehiclesDataConfig;
            _wheelsDataConfig = wheelsDataConfig;
            _cancellationTokenSource = new CancellationTokenSource();

            _storage = new Storage
            {
                Wallet = new Wallet
                {
                    Hard = profileDataConfig.hard,
                    Soft = profileDataConfig.soft
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

                var profileVehicleData = profileDataConfig.vehiclesData.FirstOrDefault(x => x.id == vehicle.Id);

                if(profileVehicleData == null)
                    continue;
                
                _storage.BoughtVehicles.Add(vehicle);
                
                if(vehicle.Id == profileDataConfig.currentVehicleId)
                    _storage.CurrentVehicle = vehicle;

                foreach (var wheelsData in profileDataConfig.wheelsData)
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
        public event Action<long> OnHardChanged;
        public event Action<long> OnSoftChanged;        
        #endregion

        #region Vehicles
        public async UniTask<List<Vehicle>> GetAllVehicles()
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.AllVehicles;
        }

        public async UniTask<List<Vehicle>> GetBoughtVehicles()
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.BoughtVehicles;
        }

        public async UniTask<Vehicle> GetCurrentVehicle()
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.CurrentVehicle;
        }

        public async UniTask<bool> SetCurrentVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            _storage.CurrentVehicle = vehicle;
            _profileDataConfig.currentVehicleId = vehicle.Id;
            return true;
        }

        public async UniTask<bool> BuyVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            if (_storage.BoughtVehicles.Contains(vehicle))
                return false;
            
            if (vehicle.BuyPrice > _storage.Wallet.Soft)
                return false;
            
            _storage.BoughtVehicles.Add(vehicle);
            return true;
        }
        #endregion Vehicles

        #region Wheels
        public async UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return vehicle.AllWheels;
        }

        public async UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return vehicle.BoughtWheels;
        }

        public async UniTask<Wheels> GetCurrentWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return vehicle.CurrentWheels;
        }

        public async UniTaskVoid SetCurrentWheels(Vehicle vehicle, Wheels wheels)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            vehicle.CurrentWheels = wheels;
            _profileDataConfig.vehiclesData.First(x => x.id == vehicle.Id).currentWheelsId = wheels.Id;
        }

        public async UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel)
        {
            await UniTask.WaitForSeconds(.1f, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }
        #endregion Wheels

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }
}
