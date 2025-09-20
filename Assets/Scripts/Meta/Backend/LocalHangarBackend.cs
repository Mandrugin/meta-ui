using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;

namespace Meta.UseCases
{
    public class LocalHangarBackend : IHangarBackend, IDisposable
    {
        private readonly List<Vehicle> _allVehicles;
        private readonly List<Vehicle> _boughtVehicles;
        private Vehicle _currentVehicle;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public LocalHangarBackend()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            _allVehicles = new List<Vehicle>()
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
            };
            foreach (var vehicle in _allVehicles)
            {
                vehicle.BoughtWheels = new List<Wheels>();
                vehicle.BoughtWheels.AddRange(vehicle.AllWheels);
                vehicle.CurrentWheels = vehicle.BoughtWheels[0];
            }
            _boughtVehicles = new List<Vehicle>();
            _boughtVehicles.AddRange(_allVehicles);
            _currentVehicle = _boughtVehicles[0];
        }


        public async UniTask<List<Vehicle>> GetAllVehicles()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _allVehicles;
        }

        public async UniTask<List<Vehicle>> GetBoughtVehicles()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _boughtVehicles;
        }

        public async UniTask<Vehicle> GetCurrentVehicle()
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            return _currentVehicle;
        }

        public async UniTask<bool> SetCurrentVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }

        public async UniTask<bool> BuyVehicle(Vehicle vehicle)
        {
            await UniTask.WaitForSeconds(1, false, PlayerLoopTiming.Update,  _cancellationTokenSource.Token);
            throw new System.NotImplementedException();
        }

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

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
