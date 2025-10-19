using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class WheelsChangingUseCase : IWheelsChangingUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly IVehicleUseCase _vehicleUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private Wheels _setWheels;
        private List<Wheels> _allCurrentWheels;
        private List<Wheels> _allBoughtWheels;

        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };
        public event Action<WheelsData> OnCurrentWheelsChanged = delegate { };
        public event Action<WheelsData> OnWheelsSet = delegate { };
        public event Action<WheelsData> OnWheelsBought = delegate { };
        public event Action<bool> OnSetAvailable = delegate { };
        public event Action<bool> OnBuyAvailable = delegate { };
        public event Action<List<WheelsData>, List<WheelsData>, WheelsData> OnWheelsListChanged = delegate { };

        public WheelsChangingUseCase(IHangarGateway hangarGateway, IVehicleUseCase vehicleUseCase)
        {
            _hangarGateway = hangarGateway;
            _vehicleUseCase = vehicleUseCase;
            _vehicleUseCase.OnCurrentVehicleChanged += OnVehicleChange;
        }

        public void Dispose()
        {
            _vehicleUseCase.OnCurrentVehicleChanged -= OnVehicleChange;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void OnVehicleChange(VehicleData vehicleData)
        {
            UpdateWheelsData(_cancellationTokenSource.Token).Forget();
        }

        public void ShowPresenter() => OnShowPresenter.Invoke();
        public void HidePresenter() => OnHidePresenter.Invoke();

        private void TryFinishUseCase()
        {
            HidePresenter();
        }

        public async UniTask<bool> TryWheelsOut(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle.Id, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            var wheels = _allCurrentWheels.FirstOrDefault(x => x.Id == wheelsData.Id);
            if (wheels == null)
            {
                Debug.LogError("No wheels found");
                return false;
            }
            _currentWheels = wheels;
            OnCurrentWheelsChanged.Invoke(wheelsData);
            OnSetAvailable.Invoke( await IsSetAvailable(wheelsData, cancellationToken));
            OnBuyAvailable.Invoke( await IsBuyAvailable(wheelsData, cancellationToken));
            return true;
        }

        public async UniTask<bool> SetWheels(CancellationToken cancellationToken)
        {
            if(_currentVehicle == null)
                throw new NullReferenceException($"{nameof(_currentVehicle)} cannot be null");
            
            if(_currentWheels == null)
                throw new NullReferenceException($"{nameof(_currentWheels)} cannot be null");
            
            if(_currentWheels == _setWheels)
                throw new InvalidOperationException("Wheels already set");
            
            var result = await _hangarGateway.SetWheels(_currentVehicle, _currentWheels, cancellationToken);

            if (!result)
                return false;
            
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle.Id, cancellationToken);
            
            _setWheels = _currentWheels;
            OnCurrentWheelsChanged(_currentWheels.ToWheelsData());
            OnWheelsSet.Invoke(_currentWheels.ToWheelsData());
            OnWheelsListChanged(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
            
            OnSetAvailable.Invoke(false);
            OnBuyAvailable.Invoke(false);

            return true;
        }

        public async UniTask<bool> BuyWheels(CancellationToken cancellationToken)
        {
            if(_currentVehicle == null)
                throw new NullReferenceException($"{nameof(_currentVehicle)} cannot be null");
            
            if(_currentWheels == null)
                throw new NullReferenceException($"{nameof(_currentWheels)} cannot be null");
            
            var result = await _hangarGateway.BuyWheels(_currentVehicle, _currentWheels, cancellationToken);
            
            if (!result)
                return false;
            
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle.Id, cancellationToken);
            
            _setWheels = _currentWheels;
            OnWheelsSet.Invoke(_currentWheels.ToWheelsData());
            OnWheelsListChanged(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
            
            OnSetAvailable.Invoke(false);
            OnBuyAvailable.Invoke(false);

            return true;
        }

        public async UniTask<WheelsData> GetSetWheels(CancellationToken  cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            _setWheels = await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            return new WheelsData
            {
                Id = _setWheels.Id,
                Price = _setWheels.Price
            };
        }

        public async UniTask UpdateWheelsData(CancellationToken cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle.Id, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            
            OnWheelsListChanged(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
        }

        public async UniTask<bool> IsSetAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle.Id, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            if (_setWheels.Id == wheelsData.Id)
                return false;

            var wheels = _allCurrentWheels.FirstOrDefault(x =>  x.Id == wheelsData.Id);
            if (wheels == null)
                throw new ArgumentException($"vehicle {_currentVehicle.Id} doesn't have wheels {wheelsData.Id}");

            return (await _hangarGateway.GetBoughtWheels(_currentVehicle.Id, cancellationToken)).Any(x => x.Id == wheelsData.Id);
        }

        public async UniTask<bool> IsBuyAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            var vehicle = await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var boughtWheels = await _hangarGateway.GetBoughtWheels(vehicle.Id, cancellationToken);
            if (boughtWheels.Any(x => x.Id == wheelsData.Id))
                return false;

            var soft = await _hangarGateway.GetSoftBalance(cancellationToken);
            return wheelsData.Price <= soft;
        }
    }
}
