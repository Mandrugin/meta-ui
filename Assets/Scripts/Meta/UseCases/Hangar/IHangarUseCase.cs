using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarUseCase
    {
        public UniTask<VehicleData> GetCurrentVehicle();
        public UniTaskVoid StartWheelsChanging();
    }
}