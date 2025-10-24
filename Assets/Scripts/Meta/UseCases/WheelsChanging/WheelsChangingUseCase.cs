using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Meta.UseCases
{
    [Preserve]
    public class WheelsChangingUseCase : IDisposable, IAsyncStartable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly UseCaseMediator _useCaseMediator;
        private readonly IWheelsChangingFactory _wheelsChangingFactory;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        private IWheelsChangingPresenter _wheelsChangingPresenter;

        private Vehicle _currentVehicle;
        private Wheels _currentWheels;
        private Wheels _setWheels;
        private List<Wheels> _allCurrentWheels;
        private List<Wheels> _allBoughtWheels;

        private UniTask<bool> _tryWheelsOutTask = UniTask.FromResult(true);
        private UniTask<bool> _setWheelsTask = UniTask.FromResult(true);
        private UniTask<bool> _buyWheelsTask = UniTask.FromResult(true);
        private UniTask _changeVehicleTask = UniTask.CompletedTask;
        private UniTask _showWheelsChangingTask = UniTask.CompletedTask;

        internal WheelsChangingUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator, IWheelsChangingFactory wheelsChangingFactory)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _wheelsChangingFactory = wheelsChangingFactory;
            
            _useCaseMediator.OnShowWheelsChanging += ShowPresenter;
            _useCaseMediator.OnHideWheelsChanging += HidePresenter;
        }

        public void Dispose()
        {
            _useCaseMediator.OnShowWheelsChanging -= ShowPresenter;
            _useCaseMediator.OnHideWheelsChanging -= HidePresenter;
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _wheelsChangingFactory.DestroyWheelsChangingPresenter();
        }

        private void ShowPresenter()
        {
            if (_wheelsChangingPresenter != null)
                return;
            
            if (_showWheelsChangingTask.Status == UniTaskStatus.Pending)
                return;

            _showWheelsChangingTask = ShowPresenterAsync(_cancellationTokenSource.Token);
        }

        private async UniTask ShowPresenterAsync(CancellationToken cancellationToken)
        {
            _wheelsChangingPresenter = await _wheelsChangingFactory.GetWheelsChangingPresenter(cancellationToken);
            _wheelsChangingPresenter.OnSetWheels += SetWheels;
            _wheelsChangingPresenter.OnBuyWheels += BuyWheels;
            _wheelsChangingPresenter.OnWheelsTriedOut += TryWheelsOut;
            _wheelsChangingPresenter.SetBuyAvailable(false);
            _wheelsChangingPresenter.SetSetAvailable(false);
            _useCaseMediator.OnCurrentVehicleChanged += OnVehicleChange;
            await UpdateWheelsData(cancellationToken);
        }

        private void HidePresenter()
        {
            if (_wheelsChangingPresenter == null)
                return;
            
            ResetWheels().Forget();

            if (_showWheelsChangingTask.Status == UniTaskStatus.Pending)
                return;


            _wheelsChangingPresenter.OnSetWheels -= SetWheels;
            _wheelsChangingPresenter.OnBuyWheels -= BuyWheels;
            _wheelsChangingPresenter.OnWheelsTriedOut -= TryWheelsOut;
            _useCaseMediator.OnCurrentVehicleChanged -= OnVehicleChange;
            _wheelsChangingFactory.DestroyWheelsChangingPresenter();
            _wheelsChangingPresenter = null;
        }

        private UniTask<bool> ResetWheels()
        {
            _currentWheels = _setWheels;
            _useCaseMediator.ChangeCurrentWheels(_currentWheels);
            return UniTask.FromResult(true);
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            // bring the use case to life
            await UniTask.WaitForSeconds(0, cancellationToken: cancellation);
        }

        private async UniTask UpdateWheelsData(CancellationToken cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            
            _wheelsChangingPresenter.ChangeWheelsList(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
        }

        #region Change Vehicle
        private void OnVehicleChange(Vehicle vehicle)
        {
            if (_changeVehicleTask.Status == UniTaskStatus.Pending)
                return;

            _changeVehicleTask = ChangeVehicleAsync(vehicle, _cancellationTokenSource.Token);
        }

        private async UniTask ChangeVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
        {
            _currentVehicle = vehicle;
            
            _allCurrentWheels = await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _allBoughtWheels = await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            _setWheels = await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            _currentWheels = _setWheels;
            
            await UpdateWheelsData(cancellationToken);
        }
        #endregion Change Vehicle

        #region Try Wheels Out
        private void TryWheelsOut(WheelsData wheelsData)
        {
            if (_tryWheelsOutTask.Status == UniTaskStatus.Pending)
                return;
            
            _tryWheelsOutTask = TryWheelsOutAsync(wheelsData, _cancellationTokenSource.Token);
        }

        private async UniTask<bool> TryWheelsOutAsync(WheelsData wheelsData, CancellationToken  cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            var wheels = _allCurrentWheels.FirstOrDefault(x => x.Id == wheelsData.Id);
            if (wheels == null)
            {
                Debug.LogError("No wheels found");
                return false;
            }
            _currentWheels = wheels;
            _useCaseMediator.ChangeCurrentWheels(_currentWheels);
            _wheelsChangingPresenter.SetSetAvailable(await IsSetAvailable(wheelsData, cancellationToken));
            _wheelsChangingPresenter.SetBuyAvailable(await IsBuyAvailable(wheelsData, cancellationToken));
            return true;
        }
        #endregion Try Wheels Out

        #region Set Wheels
        private void SetWheels()
        {
            if (_setWheelsTask.Status == UniTaskStatus.Pending)
                return;

            _setWheelsTask = SetWheelsAsync(_cancellationTokenSource.Token);
        }

        private async UniTask<bool> SetWheelsAsync(CancellationToken cancellationToken)
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
            
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            
            _setWheels = _currentWheels;
            _useCaseMediator.ChangeCurrentWheels(_currentWheels);
            _wheelsChangingPresenter.ChangeWheelsList(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
            
            _wheelsChangingPresenter.SetSetAvailable(false);
            _wheelsChangingPresenter.SetBuyAvailable(false);

            return true;
        }
        #endregion

        #region Buy Wheels
        private void BuyWheels()
        {
            if (_buyWheelsTask.Status == UniTaskStatus.Pending)
                return;
            
            _buyWheelsTask = BuyWheelsAsync(_cancellationTokenSource.Token);
        }

        private async UniTask<bool> BuyWheelsAsync(CancellationToken cancellationToken)
        {
            if(_currentVehicle == null)
                throw new NullReferenceException($"{nameof(_currentVehicle)} cannot be null");
            
            if(_currentWheels == null)
                throw new NullReferenceException($"{nameof(_currentWheels)} cannot be null");
            
            var result = await _hangarGateway.BuyWheels(_currentVehicle, _currentWheels, cancellationToken);
            
            if (!result)
                return false;
            
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _allBoughtWheels ??= await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            
            _setWheels = _currentWheels;
            _wheelsChangingPresenter.ChangeWheelsList(
                _allCurrentWheels.Select(x => x.ToWheelsData()).ToList(),
                _allBoughtWheels.Select(x => x.ToWheelsData()).ToList(),
                _setWheels.ToWheelsData());
            
            _wheelsChangingPresenter.SetSetAvailable(false);
            _wheelsChangingPresenter.SetBuyAvailable(false);

            return true;
        }
        #endregion Buy Wheels

        #region Chacking Availability
        private async UniTask<bool> IsSetAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellationToken);
            _allCurrentWheels ??= await _hangarGateway.GetAllWheels(_currentVehicle, cancellationToken);
            _setWheels ??= await _hangarGateway.GetSetWheels(_currentVehicle, cancellationToken);
            if (_setWheels.Id == wheelsData.Id)
                return false;

            var wheels = _allCurrentWheels.FirstOrDefault(x =>  x.Id == wheelsData.Id);
            if (wheels == null)
                throw new ArgumentException($"vehicle {_currentVehicle} doesn't have wheels {wheelsData.Id}");

            return (await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken)).Any(x => x.Id == wheelsData.Id);
        }

        private async UniTask<bool> IsBuyAvailable(WheelsData wheelsData, CancellationToken cancellationToken)
        {
            _currentVehicle ??= await _hangarGateway.GetSetVehicle(cancellationToken);
            var boughtWheels = await _hangarGateway.GetBoughtWheels(_currentVehicle, cancellationToken);
            if (boughtWheels.Any(x => x.Id == wheelsData.Id))
                return false;

            var soft = await _hangarGateway.GetSoftBalance(cancellationToken);
            return wheelsData.Price <= soft;
        }
        #endregion Chacking Availability
    }
}
