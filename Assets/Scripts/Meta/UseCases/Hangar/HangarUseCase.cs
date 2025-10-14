using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Meta.UseCases
{
    public class HangarUseCase : UseCase, IHangarUseCase, IDisposable
    {
        [Inject] private IHangarGateway _hangarGateway;

        public event Action OnStartWheelsChanging = delegate { };
        public event Action OnFinishWheelsChanging = delegate { };
        public event Action<WheelsData> OnTryWheelsOut;

        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public void StartWheelsChanging() => OnStartWheelsChanging.Invoke();
        public void FinishWheelsChanging() => OnFinishWheelsChanging.Invoke();
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
        }
    }
}
