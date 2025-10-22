using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class VehiclePresenter: IVehiclePresenter, IDisposable
    {
        private readonly IVehicleView _vehicleView;

        public VehiclePresenter(IVehicleView vehicleView)
        {
            _vehicleView = vehicleView;
        }

        public void Dispose()
        {
            // ...
        }

        public void ChangeVehicle(VehicleData vehicleData) => _vehicleView.ChangeVehicle(vehicleData.ToVehicleDataView());
        public void ChangeWheels(WheelsData wheelsData) => _vehicleView.ChangeWheels(wheelsData.ToWheelsDataView());
    }
}
