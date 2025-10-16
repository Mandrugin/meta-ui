using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarUseCase: IUseCase
    {
        public event Action OnStartWheelsChanging;
        public event Action OnFinishWheelsChanging;
        public event Action<long> OnHardChanged;
        public event Action<long> OnSoftChanged;
        public UniTask<long> GetHardBalance(CancellationToken cancellationToken);
        public UniTask<long> GetSoftBalance(CancellationToken cancellationToken);
        public event Action<WheelsData> OnTryWheelsOut;
        public void StartWheelsChanging();
        public void FinishWheelsChanging();
        UniTask<bool> TryWheelsOut(WheelsData wheelsData);
        public UniTask<VehicleData> GetCurrentVehicle();
    }
}
