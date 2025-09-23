using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.Gateway
{
    public class LocalHangarGateway : IHangarGateway, IDisposable
    {
        private readonly Storage _storage;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public LocalHangarGateway()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            _storage = new Storage
            {
                AllVehicles = new List<Vehicle>()
                {
                    new Vehicle()
                    {
                        Id = "firstVehicle",
                        AllWheels = new List<Wheels>()
                        {
                            new Wheels()
                            {
                                Id = "small",
                                Price = 10
                            },
                            new Wheels()
                            {
                                Id = "medium",
                                Price = 20
                            },
                            new Wheels()
                            {
                                Id = "large",
                                Price = 30
                            }
                        }
                    },
                    new Vehicle()
                    {
                        Id = "secondVehicle",
                        AllWheels = new List<Wheels>()
                        {
                            new Wheels()
                            {
                                Id = "small",
                                Price = 100
                            },
                            new Wheels()
                            {
                                Id = "medium",
                                Price = 200
                            },
                            new Wheels()
                            {
                                Id = "large",
                                Price = 300
                            }
                        }
                    }
                }
            };

            foreach (var vehicle in _storage.AllVehicles)
            {
                vehicle.BoughtWheels = new List<Wheels>();
                vehicle.BoughtWheels.AddRange(vehicle.AllWheels);
                vehicle.CurrentWheels = vehicle.BoughtWheels[0];
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
