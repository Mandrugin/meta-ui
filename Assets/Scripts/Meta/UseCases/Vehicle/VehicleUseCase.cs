using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class VehicleUseCase : IVehicleUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly UseCaseMediator _useCaseMediator;
        
        private Vehicle _currentVehicle;
        private List<Vehicle> _allVehicles;

        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };
        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };

        internal VehicleUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _useCaseMediator.OnCurrentVehicleChanged += UseCaseMediatorOnOnCurrentVehicleChanged;
        }

        public void Dispose()
        {
            _useCaseMediator.OnCurrentVehicleChanged -= UseCaseMediatorOnOnCurrentVehicleChanged;
        }

        private void UseCaseMediatorOnOnCurrentVehicleChanged(Vehicle vehicle)
        {
            OnCurrentVehicleChanged.Invoke(vehicle.ToVehicleData());
        }

        private void ChangeCurrentVehicle(Vehicle vehicle)
        {
            _useCaseMediator.ChangeCurrentVehicle(vehicle);
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

        public async UniTask UpdateVehicleData(CancellationToken token)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(token);
            if (_currentVehicle == null)
                throw new Exception("Cannot update find current vehicle");

            ChangeCurrentVehicle(_currentVehicle);
        }

        public UniTask SetNextVehicle(CancellationToken token)
        {
            return SetNearVehicle(1, token);
        }

        public UniTask SetPrevVehicle(CancellationToken token)
        {
            return SetNearVehicle(-1, token);
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
