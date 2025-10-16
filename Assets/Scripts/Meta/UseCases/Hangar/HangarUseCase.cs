using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCase : UseCase, IHangarUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        public event Action OnStartWheelsChanging = delegate { };
        public event Action OnFinishWheelsChanging = delegate { };
        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };

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
            _hangarGateway.OnHardChanged += OnOnHardChanged;
            _hangarGateway.OnSoftChanged += OnOnSoftChanged;
        }

        public void StartWheelsChanging() => OnStartWheelsChanging.Invoke();
        public void FinishWheelsChanging() => OnFinishWheelsChanging.Invoke();
        private void OnOnHardChanged(long hard) => OnHardChanged.Invoke(hard);
        private void OnOnSoftChanged(long soft) => OnSoftChanged.Invoke(soft);

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle(cancellationToken);
            return new VehicleData
            {
                Id = vehicle.Id
            };
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            
            _hangarGateway.OnHardChanged -= OnOnHardChanged;
            _hangarGateway.OnSoftChanged -= OnOnSoftChanged;
        }
    }
}
