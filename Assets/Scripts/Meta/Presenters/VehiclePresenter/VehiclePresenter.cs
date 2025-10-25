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

        public void SetCommonPosition() => _vehicleView.SetCommonPosition();
        public void SetWheelsChangingPosition() => _vehicleView.SetWheelsChangingPosition();
        
        public void ShowVehicle() => _vehicleView.ShowVehicle();
        public void HideVehicle() => _vehicleView.HideVehicle();
    }
}
