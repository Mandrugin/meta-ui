using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Meta.UseCases
{
    public class HangarUseCaseLogDecorator : IHangarUseCase
    {
        private readonly IHangarUseCase _hangarUseCase;
        private readonly ILogger _logger;
        
        public HangarUseCaseLogDecorator(IHangarUseCase hangarUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _logger = Debug.unityLogger;

            _logger.Log("HangarCuseCase Creating...");
            
            _hangarUseCase.OnStartUseCase += StartUseCaseInvocator;
            _hangarUseCase.OnFinishUseCase += FinishUseCaseInvocator;
            _hangarUseCase.OnStartWheelsChanging += StartWheelsChangingInvocator;
            _hangarUseCase.OnFinishWheelsChanging += FinishWheelsChangingInvocator;

            _logger.Log("HangarUseCase Created");
        }

        ~HangarUseCaseLogDecorator()
        {
            _logger.Log("HangarUseCase Destroying...");
            _hangarUseCase.OnStartUseCase -= StartUseCaseInvocator;
            _hangarUseCase.OnFinishUseCase -= FinishUseCaseInvocator;
            _hangarUseCase.OnStartWheelsChanging -= StartWheelsChangingInvocator;
            _hangarUseCase.OnFinishWheelsChanging -= FinishWheelsChangingInvocator;
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

        public event Action OnStartWheelsChanging;
        public event Action OnFinishWheelsChanging;

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

        public async UniTask<VehicleData> GetCurrentVehicle()
        {
            _logger.Log("HangarUseCase GetCurrentVehicle");
            return await _hangarUseCase.GetCurrentVehicle();
        }
    }
}