using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCase : IHangarUseCase, IDisposable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };
        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };
        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };
        
        private Vehicle _currentVehicle;

        public HangarUseCase(IHangarGateway hangarGateway, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarGateway = hangarGateway;
            _wheelsChangingUseCase = wheelsChangingUseCase;
        }
        
        public void Dispose()
        {
        }
        
        public void ShowPresenter()
        {
            OnShowPresenter.Invoke();
        }

        public void HidePresenter()
        {
            OnHidePresenter.Invoke();
        }

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetHardBalance(cancellationToken);
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await _hangarGateway.GetSoftBalance(cancellationToken);
        }

        public void StartWheelsChanging() => _wheelsChangingUseCase.ShowPresenter();
        public void FinishWheelsChanging() => _wheelsChangingUseCase.HidePresenter();

        public async UniTask<VehicleData> GetCurrentVehicle(CancellationToken cancellationToken)
        {
            var previousVehicle = _currentVehicle;
            _currentVehicle ??= await _hangarGateway.GetCurrentVehicle(cancellationToken);
            var vehicleData = new VehicleData
            {
                Id = _currentVehicle.Id
            };
            if(previousVehicle != _currentVehicle)
                OnCurrentVehicleChanged.Invoke(vehicleData);
            return vehicleData;
        }
    }
}
