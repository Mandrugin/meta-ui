using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingUseCase : IUseCase
    {
        public event Action<WheelsData> OnTriedOutWheels;
        UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> IsBuyAvailable(WheelsData vehicleData, CancellationToken cancellationToken);
        UniTask<bool> BuyWheels(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> IsSetAvailable(WheelsData wheelsData, CancellationToken cancellationToken);
        UniTask<bool> SetWheels(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetAllWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetBoughtWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<WheelsData> GetCurrentWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
    }
}
