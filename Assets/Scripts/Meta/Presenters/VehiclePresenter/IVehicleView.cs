namespace Meta.Presenters
{
    public interface IVehicleView
    {
        void ChangeVehicle(VehicleDataView vehicleDataView);
        void ChangeWheels(WheelsDataView wheelsDataView);
    }
}