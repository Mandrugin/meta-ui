using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingUseCase : IUseCase
    {
        UniTask<bool> IsBuyAvailable(WheelsData wheelsData, CancellationToken cancellationToken);
        UniTask<bool> IsSetAvailable(WheelsData wheelsData, CancellationToken cancellationToken);
        public event Action<WheelsData> OnWheelsTriedOut;
        public event Action<WheelsData> OnWheelsSet;
        public event Action<WheelsData> OnWheelsBought;
        public event Action<List<WheelsData>, List<WheelsData>, WheelsData> OnWheelsListChanged;
        UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> SetWheels(CancellationToken cancellationToken);
        UniTask<bool> BuyWheels(CancellationToken cancellationToken);
        UniTask<WheelsData> GetSetWheels(CancellationToken  cancellationToken);
        UniTask UpdateWheelsDataView(CancellationToken cancellationToken);
    }
}
