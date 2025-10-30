using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using VContainer.Unity;

namespace Meta.UseCases
{
    [UnityEngine.Scripting.Preserve]
    public class VehicleUseCase : IDisposable, IAsyncStartable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly UseCaseMediator _useCaseMediator;
        private readonly IVehicleFactory _vehicleFactory;
        private readonly IOverlayLoadingFactory _overlayLoadingFactory;
        
        private IVehiclePresenter _vehiclePresenter;
        
        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private List<Vehicle> _allVehicles;

        internal VehicleUseCase(IHangarGateway hangarGateway,
            UseCaseMediator useCaseMediator,
            IVehicleFactory vehicleFactory,
            IOverlayLoadingFactory overlayLoadingFactory)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _vehicleFactory = vehicleFactory;
            _overlayLoadingFactory = overlayLoadingFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            _vehiclePresenter = await _vehicleFactory.GetVehiclePresenter(cancellation);
            _useCaseMediator.OnCurrentVehicleChanged += ChangeCurrentVehicle;
            _useCaseMediator.OnCurrentWheelsChanged += ChangeCurrentWheels;
            _useCaseMediator.OnShowWheelsChanging += OnShowWheelsChanging;
            _useCaseMediator.OnHideWheelsChanging += OnHideWheelsChanging;

            var vehicle = await _hangarGateway.GetSetVehicle(cancellation);
            if (vehicle == null)
                throw new Exception("Cannot update find current vehicle");

            ChangeCurrentVehicle(vehicle);
        }

        public void Dispose()
        {
            _useCaseMediator.OnCurrentVehicleChanged -= ChangeCurrentVehicle;
            _useCaseMediator.OnCurrentWheelsChanged -= ChangeCurrentWheels;
            _useCaseMediator.OnShowWheelsChanging -= OnShowWheelsChanging;
            _useCaseMediator.OnHideWheelsChanging -= OnHideWheelsChanging;
        }

        private void ChangeCurrentVehicle(Vehicle vehicle)
        {
            if (_currentVehicle == vehicle)
                return;
            
            _currentVehicle = vehicle;
            _currentWheels = vehicle.CurrentWheels;

            ChangeCurrentVehicleAsync(_currentVehicle).Forget();
        }

        private async UniTask ChangeCurrentVehicleAsync(Vehicle vehicle)
        {
            var overlayLoadingPresenter = await _overlayLoadingFactory.GetOverlayLoadingPresenter(CancellationToken.None);
            _vehiclePresenter.HideVehicle();
            await _vehiclePresenter.ChangeVehicle(vehicle.ToVehicleData());
            await _vehiclePresenter.ChangeWheels(vehicle.CurrentWheels.ToWheelsData());
            _vehiclePresenter.ShowVehicle();
            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlayLoadingPresenter);
        }

        private void ChangeCurrentWheels(Wheels wheels)
        {
            if (_currentWheels == wheels)
                return;

            _currentWheels = wheels;
            _vehiclePresenter.ChangeWheels(wheels.ToWheelsData());
        }

        private void OnShowWheelsChanging() => _vehiclePresenter.SetWheelsChangingPosition();
        private void OnHideWheelsChanging() => _vehiclePresenter.SetCommonPosition();
    }
}
