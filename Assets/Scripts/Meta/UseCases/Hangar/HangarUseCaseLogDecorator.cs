using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCaseLogDecorator : IHangarUseCase
    {
        private readonly IHangarUseCase _hangarUseCase;
        private readonly ILogger _logger;
        
        public HangarUseCaseLogDecorator(IHangarUseCase hangarUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _logger = Debug.unityLogger;

            _logger.Log("HangarCuseCase Creating...");
            
            _hangarUseCase.OnCurrentVehicleChanged += OnCurrentVehicleChangedInvocator;
            _hangarUseCase.OnShowPresenter += ShowPresenterInvocator;
            _hangarUseCase.OnHidePresenter += HidePresenterInvocator;
            _hangarUseCase.OnHardChanged += OnHardChangedInvocator;
            _hangarUseCase.OnSoftChanged += OnSoftChangedInvocator;

            _logger.Log("HangarUseCase Created");
        }

        ~HangarUseCaseLogDecorator()
        {
            _logger.Log("HangarUseCase Destroying...");
            _hangarUseCase.OnCurrentVehicleChanged -= OnCurrentVehicleChangedInvocator;
            _hangarUseCase.OnShowPresenter -= ShowPresenterInvocator;
            _hangarUseCase.OnHidePresenter -= HidePresenterInvocator;
            _hangarUseCase.OnHardChanged -= OnHardChangedInvocator;
            _hangarUseCase.OnSoftChanged -= OnSoftChangedInvocator;
            _logger.Log("HangarUseCase Destroyed");
        }

        public event Action OnShowPresenter;

        public event Action OnHidePresenter;

        private void ShowPresenterInvocator()
        {
            _logger.Log("HangarUseCase OnStartUseCase");
            OnShowPresenter?.Invoke();
        }

        private void HidePresenterInvocator()
        {
            _logger.Log("HangarUseCase OnFinishUseCase");
            OnHidePresenter?.Invoke();
        }

        public void ShowPresenter()
        {
            _logger.Log("HangarUseCase StartUseCase");
            _hangarUseCase.ShowPresenter();
        }

        public void HidePresenter()
        {
            _logger.Log("HangarUseCase FinishUseCase");
            _hangarUseCase.HidePresenter();
        }

        public event Action<VehicleData> OnCurrentVehicleChanged;
        public event Action<long> OnHardChanged;
        public event Action<long> OnSoftChanged;

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _hangarUseCase.GetHardBalance(cancellationToken);
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await _hangarUseCase.GetSoftBalance(cancellationToken);
        }

        private void OnCurrentVehicleChangedInvocator(VehicleData vehicleData)
        {
            OnCurrentVehicleChanged?.Invoke(vehicleData);
        }

        private void OnHardChangedInvocator(long hard)
        {
            _logger.Log($"HangarUseCase OnHardChanged {hard}");
            OnHardChanged?.Invoke(hard);
        }

        private void OnSoftChangedInvocator(long soft)
        {
            _logger.Log($"HangarUseCase OnSoftChanged {soft}");
            OnSoftChanged?.Invoke(soft);
        }
        
        public void StartWheelsChanging()
        {
            _logger.Log("HangarUseCase StartWheelsChanging");
            _hangarUseCase.StartWheelsChanging();
        }

        public void FinishWheelsChanging()
        {
            _logger.Log("HangarUseCase FinishWheelsChanging");
            _hangarUseCase.FinishWheelsChanging();
        }

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            _logger.Log("HangarUseCase GetCurrentVehicle");
            return await _hangarUseCase.GetCurrentVehicle(cancellationToken);
        }
    }
}