using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCase : UseCase, IHangarUseCase, IDisposable, IUseCase
    {
        private readonly IHangarGateway _hangarGateway;
        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };
        public event Action OnStartWheelsChanging = delegate { };
        public event Action OnFinishWheelsChanging = delegate { };
        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };
        
        private Vehicle _currentVehicle;

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetHardBalance(cancellationToken);
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetSoftBalance(cancellationToken);
        }

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public HangarUseCase(IHangarGateway hangarGateway)
        {
            _hangarGateway = hangarGateway;
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

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
    }
}
