using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace UseCases
{
    public interface IWheelsChangingUseCase
    {
        UniTask<bool> BuyWheels(int wheelsIndex);
        UniTask<bool> TryWheelsOut(int wheelsIndex);
        UniTask<bool> SetWheels(int wheelsIndex);
        UniTask<List<WheelsData>> GetAllWheels();
        UniTask<List<WheelsData>> GetBoughtWheels();
        UniTask<WheelsData> GetCurrentWheels();
    }
}
