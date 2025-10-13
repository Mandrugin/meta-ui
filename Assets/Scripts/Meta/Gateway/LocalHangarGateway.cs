using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using Meta.UseCases;

namespace Meta.Gateway
{
    public class LocalHangarGateway : IHangarGateway, IDisposable
    {
        private readonly Storage _storage;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public LocalHangarGateway(VehiclesDataConfig vehiclesDataConfig, WheelsDataConfig wheelsDataConfig)
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _storage = new Storage
            {
                AllVehicles = new(),
                Wallet = new Wallet()
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
                    if(wheelsData.vehicleId != vehicleData.id)
                        continue;

                    var wheels = new Wheels
                    {
                        Id = wheelsData.id,
                        Price = wheelsData.price,
                    };
                    
                    vehicle.AllWheels.Add(wheels);
                }
                
                vehicle.BoughtWheels = new List<Wheels>();
                vehicle.BoughtWheels.AddRange(vehicle.AllWheels);
                vehicle.CurrentWheels = vehicle.BoughtWheels.Count > 0 ? vehicle.BoughtWheels[0] : null;
                
                _storage.AllVehicles.Add(vehicle);
            }

            _storage.BoughtVehicles = new List<Vehicle>();
            _storage.BoughtVehicles.AddRange(_storage.AllVehicles);
            _storage.CurrentVehicle = _storage.BoughtVehicles[0];
        }

        #region Vehicles
        public async UniTask<List<Vehicle>> GetAllVehicles()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.AllVehicles;
        }

        public async UniTask<List<Vehicle>> GetBoughtVehicles()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.BoughtVehicles;
        }

        public async UniTask<Vehicle> GetCurrentVehicle()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _storage.CurrentVehicle;
        }

        public async UniTask<bool> SetCurrentVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            _storage.CurrentVehicle = vehicle;
            return true;
        }

        public async UniTask<bool> BuyVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            if (vehicle.BuyPrice > _storage.Wallet.Soft)
                return false;
            
            _storage.BoughtVehicles.Add(vehicle);
            return true;
        }
        #endregion Vehicles

        #region Wheels
        public async UniTask<List<Wheels>> GetAllWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return vehicle.AllWheels;
        }

        public async UniTask<List<Wheels>> GetBoughtWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }

        public async UniTask<Wheels> GetCurrentWheels(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }

        public async UniTaskVoid SetCurrentWheels(Vehicle vehicle, Wheels wheels)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }

        public async UniTask<bool> BuyWheels(Vehicle vehicle, Wheels wheel)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }
        #endregion Wheels

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
