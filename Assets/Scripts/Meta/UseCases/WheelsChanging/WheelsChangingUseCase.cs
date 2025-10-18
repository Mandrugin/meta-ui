using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class WheelsChangingUseCase : UseCase, IWheelsChangingUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly IHangarUseCase _hangarUseCase;

        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private Wheels _setWheels;
        private List<Wheels> _allCurrentWheels;

        public event Action<WheelsData> OnWheelsTriedOut = delegate { };
        public event Action<WheelsData> OnWheelsSet = delegate { };
        public event Action<WheelsData> OnWheelsBought = delegate { };

        public WheelsChangingUseCase(IHangarGateway hangarGateway, IHangarUseCase hangarUseCase)
        {
            _hangarGateway = hangarGateway;
            _hangarUseCase = hangarUseCase;
            _hangarUseCase.OnStartWheelsChanging += StartUseCase;
            _hangarUseCase.OnFinishWheelsChanging += TryFinishUseCase;
        }
        
        public void Dispose()
        {
            _hangarUseCase.OnStartWheelsChanging -= StartUseCase;
            _hangarUseCase.OnFinishWheelsChanging -= TryFinishUseCase;
        }

        private void TryFinishUseCase()
        {
            FinishUseCase();
        }

        public async UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            var wheels = _allCurrentWheels.FirstOrDefault(x => x.Id == wheelsData.Id);
            if (wheels == null)
                return false;
            _setWheels = wheels;
            OnWheelsTriedOut.Invoke(wheelsData);
            return true;
        }

        public async UniTask<bool> SetWheels(CancellationToken cancellationToken)
        {
            if(_currentVehicle == null)
                throw new NullReferenceException($"{nameof(_currentVehicle)} cannot be null");
            
            if(_currentWheels == null)
                throw new NullReferenceException($"{nameof(_currentWheels)} cannot be null");
            
            if(_currentWheels == _setWheels)
                throw new InvalidOperationException("Wheels already set");
            
            var result = await _hangarGateway.SetCurrentWheels(_currentVehicle, _currentWheels, cancellationToken);

            if (!result)
                return false;
            
            _currentWheels = _setWheels;
            OnWheelsSet.Invoke(_currentWheels.ToWheelsData());

            return true;
        }

        public UniTask<bool> BuyWheels(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async UniTask<List<WheelsData>> GetAllWheels(VehicleData vehicleData, CancellationToken  cancellationToken)
        {
            var wheelsDataList = new List<WheelsData>();
            var wheels = await _hangarGateway.GetAllWheels(vehicleData.Id, cancellationToken);
            wheels.ForEach(x => wheelsDataList.Add(new WheelsData
            {
                Id = x.Id,
                Price = x.Price
            }));
            return wheelsDataList;
        }

        public async UniTask<List<WheelsData>> GetBoughtWheels(VehicleData vehicleData, CancellationToken  cancellationToken)
        {
            var wheelsDataList = new List<WheelsData>();
            var wheels = await _hangarGateway.GetBoughtWheels(vehicleData.Id, cancellationToken);
            wheels.ForEach(x => wheelsDataList.Add(new WheelsData
            {
                Id = x.Id,
                Price = x.Price
            }));
            return wheelsDataList;
        }

        public async UniTask<WheelsData> GetCurrentWheels(CancellationToken  cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var currentWheels = await _hangarGateway.GetCurrentWheels(_currentVehicle, cancellationToken);
            return new WheelsData
            {
                Id = currentWheels.Id,
                Price = currentWheels.Price
            };
        }

        public async UniTask<bool> IsSetAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var wheels = (await _hangarGateway.GetAllWheels(vehicle.Id, cancellationToken)).FirstOrDefault(x =>  x.Id == wheelsData.Id);
            if (wheels == null)
            {
                throw new ArgumentException($"vehicle {vehicle.Id} doesn't have wheels {wheelsData.Id}");
            }
            
            return (await _hangarGateway.GetBoughtWheels(vehicle.Id, cancellationToken)).Any(x => x.Id == wheelsData.Id);
        }

        public async UniTask<bool> IsBuyAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var boughtWheels = await _hangarGateway.GetBoughtWheels(vehicle.Id, cancellationToken);
            return boughtWheels.All(x => x.Id != wheelsData.Id);
        }
    }
}
