using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehicleUseCase : IUseCase
    {
        public event Action<VehicleData> OnCurrentVehicleChanged;
        public void ChangeCurrentVehicle(VehicleData vehicleData);
        UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken);
        UniTask UpdateVehicleData(CancellationToken token);
        UniTask SetNextVehicle(CancellationToken token);
        UniTask SetPrevVehicle(CancellationToken token);
    }
}