using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehicleUseCase : IUseCase
    {
        public UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken);

        UniTask UpdateVehicleData(CancellationToken token);
    }
}