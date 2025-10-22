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
        private IVehicleNavigationPresenter _vehicleNavigationPresenter;
        
        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private List<Vehicle> _allVehicles;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private UniTask _vehicleNextTask = UniTask.CompletedTask;
        private UniTask _vehiclePrevTask = UniTask.CompletedTask;

        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };

        internal VehicleUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator, IVehicleFactory vehicleFactory)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _vehicleFactory = vehicleFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            _vehiclePresenter = await _vehicleFactory.GetVehiclePresenter(cancellation);
            _vehicleNavigationPresenter = await _vehicleFactory.GetVehicleNavigationPresenter(cancellation);

            _vehicleNavigationPresenter.OnNextVehicle += SetNextVehicle;
            _vehicleNavigationPresenter.OnPrevVehicle += SetPrevVehicle;
            _useCaseMediator.OnCurrentWheelsChanged += OnOnCurrentWheelsChanged;
            
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellation);
            if (_currentVehicle == null)
                throw new Exception("Cannot update find current vehicle");

            ChangeCurrentVehicle(_currentVehicle);
            
            _currentWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellation);
            if (_currentWheels == null)
                throw new Exception("Cannot update find current vehicle");
            
            OnOnCurrentWheelsChanged(_currentWheels);
        }

        public void Dispose()
        {
            _vehicleNavigationPresenter.OnNextVehicle -= SetNextVehicle;
            _vehicleNavigationPresenter.OnPrevVehicle -= SetPrevVehicle;
        }

        private void OnOnCurrentWheelsChanged(Wheels wheels)
        {
            _currentWheels = wheels;
            _vehiclePresenter.ChangeWheels(wheels.ToWheelsData());
        }

        private void ChangeCurrentVehicle(Vehicle vehicle)
        {
            _useCaseMediator.ChangeCurrentVehicle(vehicle);
            _vehiclePresenter.ChangeVehicle(vehicle.ToVehicleData());
        }

        public void ShowPresenter()
        {
            OnShowPresenter.Invoke();
        }

        public void HidePresenter()
        {
            OnHidePresenter.Invoke();
        }

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            return await UniTask.FromResult(new VehicleData());
        }

        private void SetNextVehicle()
        {
            if (_vehicleNextTask.Status == UniTaskStatus.Pending)
                return;
            
            _vehicleNextTask = SetNearVehicle(1, _cancellationTokenSource.Token);
        }

        private void SetPrevVehicle()
        {
            if (_vehiclePrevTask.Status == UniTaskStatus.Pending)
                return;
            
            _vehiclePrevTask = SetNearVehicle(-1, _cancellationTokenSource.Token);
        }

        private async UniTask SetNearVehicle(int near, CancellationToken token)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(token);
            _allVehicles ??= await _hangarGateway.GetAllVehicles(token);
            if (_allVehicles.Count == 0)
                throw new Exception("There are no vehicles");
            
            if(!_allVehicles.Contains(_currentVehicle))
                throw new Exception($"There is no such vehicle {_currentVehicle.Id}");
            
            var index = _allVehicles.FindIndex(x => x == _currentVehicle);
            if(index == -1)
                throw new Exception($"There is such vehicle ({_currentVehicle.Id}), but something went wrong");

            index += near;
            if(index >= _allVehicles.Count)
                index = 0;
            else if(index < 0)
                index = _allVehicles.Count - 1;
            
            _currentVehicle = _allVehicles[index];
            ChangeCurrentVehicle(_currentVehicle);
            var setWheels = await _hangarGateway.GetSetWheels(_currentVehicle, token);
            if (setWheels != null)
            {
                _useCaseMediator.ChangeCurrentWheels(setWheels);
            }
        }
    }
}
