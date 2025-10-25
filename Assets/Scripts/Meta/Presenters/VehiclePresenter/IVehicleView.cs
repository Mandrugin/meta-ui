using Cysharp.Threading.Tasks;

namespace Meta.Presenters
{
    public interface IVehicleView
    {
        UniTask ChangeVehicle(VehicleDataView vehicleDataView);
        UniTask ChangeWheels(WheelsDataView wheelsDataView);
        void SetCommonPosition();
        void SetWheelsChangingPosition();
        void ShowVehicle();
        void HideVehicle();
    }
}