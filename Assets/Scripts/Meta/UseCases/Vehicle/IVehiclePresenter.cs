namespace Meta.UseCases
{
    public interface IVehiclePresenter
    {
        public void ChangeWheels(WheelsData wheelsData);
        public void ChangeVehicle(VehicleData vehicleData);
    }
}