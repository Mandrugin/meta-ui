using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingUseCase : IUseCase
    {
        public event Action<WheelsData> OnWheelsSet;
        public event Action<WheelsData> OnWheelsBought;
        public event Action<bool> OnSetAvailable;
        public event Action<bool> OnBuyAvailable;
        UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> SetWheels(CancellationToken cancellationToken);
        UniTask<bool> BuyWheels(CancellationToken cancellationToken);
        UniTask UpdateWheelsData(CancellationToken cancellationToken);
    }
}
