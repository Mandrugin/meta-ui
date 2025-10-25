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
        
        private IVehiclePresenter _vehiclePresenter;
        
        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private List<Vehicle> _allVehicles;

        internal VehicleUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator, IVehicleFactory vehicleFactory)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _vehicleFactory = vehicleFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            _vehiclePresenter = await _vehicleFactory.GetVehiclePresenter(cancellation);
            _useCaseMediator.OnCurrentVehicleChanged += ChangeCurrentVehicle;
            _useCaseMediator.OnCurrentWheelsChanged += ChangeCurrentWheels;
            _useCaseMediator.OnShowWheelsChanging += OnShowWheelsChanging;
            _useCaseMediator.OnHideWheelsChanging += OnHideWheelsChanging;

            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellation);
            if (_currentVehicle == null)
                throw new Exception("Cannot update find current vehicle");

            _currentWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellation);
            if (_currentWheels == null)
                throw new Exception("Cannot update find current vehicle");

            ChangeCurrentVehicle(_currentVehicle);
            ChangeCurrentWheels(_currentWheels);
        }

        public void Dispose()
        {
            _useCaseMediator.OnCurrentWheelsChanged -= ChangeCurrentWheels;
            _useCaseMediator.OnCurrentVehicleChanged -= ChangeCurrentVehicle;
            _useCaseMediator.OnShowWheelsChanging -= OnShowWheelsChanging;
            _useCaseMediator.OnHideWheelsChanging -= OnHideWheelsChanging;
        }

        private void ChangeCurrentVehicle(Vehicle vehicle)
        {
            _currentVehicle = vehicle;
            _vehiclePresenter.ChangeVehicle(vehicle.ToVehicleData());
        }

        private void ChangeCurrentWheels(Wheels wheels)
        {
            _currentWheels = wheels;
            _vehiclePresenter.ChangeWheels(wheels.ToWheelsData());
        }

        private void OnShowWheelsChanging() => _vehiclePresenter.SetWheelsChangingPosition();
        private void OnHideWheelsChanging() => _vehiclePresenter.SetCommonPosition();
    }
}
