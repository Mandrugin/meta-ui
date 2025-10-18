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
            _hangarUseCase.OnStartUseCase += StartUseCaseInvocator;
            _hangarUseCase.OnFinishUseCase += FinishUseCaseInvocator;
            _hangarUseCase.OnStartWheelsChanging += StartWheelsChangingInvocator;
            _hangarUseCase.OnFinishWheelsChanging += FinishWheelsChangingInvocator;
            _hangarUseCase.OnHardChanged += OnHardChangedInvocator;
            _hangarUseCase.OnSoftChanged += OnSoftChangedInvocator;

            _logger.Log("HangarUseCase Created");
        }

        ~HangarUseCaseLogDecorator()
        {
            _logger.Log("HangarUseCase Destroying...");
            _hangarUseCase.OnCurrentVehicleChanged -= OnCurrentVehicleChangedInvocator;
            _hangarUseCase.OnStartUseCase -= StartUseCaseInvocator;
            _hangarUseCase.OnFinishUseCase -= FinishUseCaseInvocator;
            _hangarUseCase.OnStartWheelsChanging -= StartWheelsChangingInvocator;
            _hangarUseCase.OnFinishWheelsChanging -= FinishWheelsChangingInvocator;
            _hangarUseCase.OnHardChanged -= OnHardChangedInvocator;
            _hangarUseCase.OnSoftChanged -= OnSoftChangedInvocator;
            _logger.Log("HangarUseCase Destroyed");
        }

        public event Action OnStartUseCase;

        public event Action OnFinishUseCase;

        private void StartUseCaseInvocator()
        {
            _logger.Log("HangarUseCase OnStartUseCase");
            OnStartUseCase?.Invoke();
        }

        private void FinishUseCaseInvocator()
        {
            _logger.Log("HangarUseCase OnFinishUseCase");
            OnFinishUseCase?.Invoke();
        }

        public void StartUseCase()
        {
            _logger.Log("HangarUseCase StartUseCase");
            _hangarUseCase.StartUseCase();
        }

        public void FinishUseCase()
        {
            _logger.Log("HangarUseCase FinishUseCase");
            _hangarUseCase.FinishUseCase();
        }

        public event Action<VehicleData> OnCurrentVehicleChanged;
        public event Action OnStartWheelsChanging;
        public event Action OnFinishWheelsChanging;
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

        private void StartWheelsChangingInvocator()
        {
            _logger.Log("HangarUseCase OnStartWheelsChanging");
            OnStartWheelsChanging?.Invoke();
        }

        private void FinishWheelsChangingInvocator()
        {
            _logger.Log("HangarUseCase OnFinishWheelsChanging");
            OnFinishWheelsChanging?.Invoke();
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