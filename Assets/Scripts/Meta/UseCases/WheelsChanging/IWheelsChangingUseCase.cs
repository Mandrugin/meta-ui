using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingUseCase : IUseCase
    {
        UniTask<bool> BuyWheels(int wheelsIndex, CancellationToken  cancellationToken);
        UniTask<bool> TryWheelsOut(int wheelsIndex, CancellationToken  cancellationToken);
        UniTask<bool> SetWheels(int wheelsIndex, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetAllWheels(CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetBoughtWheels(CancellationToken  cancellationToken);
        UniTask<WheelsData> GetCurrentWheels(CancellationToken  cancellationToken);
    }
}
