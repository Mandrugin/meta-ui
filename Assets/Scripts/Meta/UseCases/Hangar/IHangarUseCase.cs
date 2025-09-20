using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarUseCase
    {
        public UniTaskVoid StartWheelsChanging();
        public UniTask<VehicleData> GetCurrentVehicle();
    }
}