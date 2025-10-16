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
        event Action<WheelsData> OnTryWheelsOut;
        
        public void TryWheelsOut(WheelsData wheelsData);
        public UniTask<long> GetHardBalance(CancellationToken cancellationToken);
        public UniTask<long> GetSoftBalance(CancellationToken cancellationToken);
        public void StartWheelsChanging();
        public void FinishWheelsChanging();
        public UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken);
    }
}
