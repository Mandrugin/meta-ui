using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class VehicleNavigationPresenter : IVehicleNavigationPresenter, IDisposable
    {
        public event Action OnNextVehicle = delegate { };
        public event Action OnPrevVehicle = delegate { };

        private readonly IVehicleNavigationView _vehicleNavigation;

        public VehicleNavigationPresenter(IVehicleNavigationView vehicleNavigation)
        {
            _vehicleNavigation = vehicleNavigation;
            _vehicleNavigation.OnNextVehicle += InvokeOnNextVehicle;
            _vehicleNavigation.OnPrevVehicle += InvokeOnPrevVehicle;
        }

        public void Dispose()
        {
            _vehicleNavigation.OnNextVehicle -= InvokeOnNextVehicle;
            _vehicleNavigation.OnPrevVehicle -= InvokeOnPrevVehicle;
        }
        
        private void InvokeOnNextVehicle() => OnNextVehicle();
        private void InvokeOnPrevVehicle() => OnPrevVehicle();

        public void SetVehicleName(VehicleData vehicleData) =>
            _vehicleNavigation.SetVehicleName(vehicleData.ToVehicleDataView());
    }
}