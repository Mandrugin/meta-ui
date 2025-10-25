using System;
using Cysharp.Threading.Tasks;
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

        public async UniTask ChangeVehicle(VehicleData vehicleData) => await _vehicleView.ChangeVehicle(vehicleData.ToVehicleDataView());
        public async UniTask ChangeWheels(WheelsData wheelsData) => await _vehicleView.ChangeWheels(wheelsData.ToWheelsDataView());

        public void SetCommonPosition() => _vehicleView.SetCommonPosition();
        public void SetWheelsChangingPosition() => _vehicleView.SetWheelsChangingPosition();
        
        public void ShowVehicle() => _vehicleView.ShowVehicle();
        public void HideVehicle() => _vehicleView.HideVehicle();
    }
}
