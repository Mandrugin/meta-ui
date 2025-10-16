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
        public event Action<WheelsData> OnTryWheelsOut;

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

        public async UniTask<bool> TryWheelsOut(WheelsData wheelsData)
        {
            OnTryWheelsOut?.Invoke(wheelsData);
            return await UniTask.FromResult(true);
        }

        public async UniTask<VehicleData> GetCurrentVehicle()
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle();
            return new VehicleData
            {
                Id = vehicle.Id,
                WheelsData = new WheelsData
                {
                    Id = vehicle.CurrentWheels.Id,
                    Price = vehicle.CurrentWheels.Price
                }
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
