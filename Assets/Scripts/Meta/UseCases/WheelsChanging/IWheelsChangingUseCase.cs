using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingUseCase : IUseCase
    {
        UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> BuyWheels(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<bool> SetWheels(WheelsData wheelsData, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetAllWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<List<WheelsData>> GetBoughtWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
        UniTask<WheelsData> GetCurrentWheels(VehicleData vehicleData, CancellationToken  cancellationToken);
    }
}
