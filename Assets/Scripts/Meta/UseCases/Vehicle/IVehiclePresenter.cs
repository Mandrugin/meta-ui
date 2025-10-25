using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehiclePresenter
    {
        public UniTask ChangeVehicle(VehicleData vehicleData);
        public UniTask ChangeWheels(WheelsData wheelsData);
        public void SetCommonPosition();
        public void SetWheelsChangingPosition();
        public void ShowVehicle();
        public void HideVehicle();
    }
}