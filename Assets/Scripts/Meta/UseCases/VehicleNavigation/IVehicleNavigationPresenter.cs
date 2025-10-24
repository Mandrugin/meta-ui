using System;

namespace Meta.UseCases
{
    public interface IVehicleNavigationPresenter
    {
        public event Action OnNextVehicle;
        public event Action OnPrevVehicle;
        public void SetVehicleName(VehicleData vehicleData);
    }
}