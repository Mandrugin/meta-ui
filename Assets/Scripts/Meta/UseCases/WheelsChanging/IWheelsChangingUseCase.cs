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
        UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> SetWheels(CancellationToken cancellationToken);
        UniTask<bool> BuyWheels(CancellationToken cancellationToken);
        UniTask<List<WheelsData>> GetAllWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetBoughtWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<WheelsData> GetCurrentWheels(CancellationToken  cancellationToken);
    }
}
