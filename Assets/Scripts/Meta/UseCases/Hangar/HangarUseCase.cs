using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCase : IHangarUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        public event Action OnStartUseCase = delegate { };
        public event Action OnFinishUseCase = delegate { };
        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };
        public event Action OnStartWheelsChanging = delegate { };
        public event Action OnFinishWheelsChanging = delegate { };
        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };
        
        private Vehicle _currentVehicle;

        public HangarUseCase(IHangarGateway hangarGateway)
        {
            _hangarGateway = hangarGateway;
        }
        
        public void Dispose()
        {
        }
        
        public void StartUseCase()
        {
            OnStartUseCase.Invoke();
        }

        public void FinishUseCase()
        {
            OnFinishUseCase.Invoke();
        }

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetHardBalance(cancellationToken);
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetSoftBalance(cancellationToken);
        }

        public void StartWheelsChanging() => OnStartWheelsChanging.Invoke();
        public void FinishWheelsChanging() => OnFinishWheelsChanging.Invoke();

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            var previousVehicle = _currentVehicle;
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var vehicleData = new VehicleData
            {
                Id = _currentVehicle.Id
            };
            if(previousVehicle != _currentVehicle)
                OnCurrentVehicleChanged.Invoke(vehicleData);
            return vehicleData;
        }
    }
}
