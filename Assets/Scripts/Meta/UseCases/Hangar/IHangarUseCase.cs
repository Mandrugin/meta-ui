using Cysharp.Threading.Tasks;

namespace UseCases
{
    public interface IHangarUseCase
    {
        public UniTask<VehicleData> GetCurrentVehicle();
    }
}