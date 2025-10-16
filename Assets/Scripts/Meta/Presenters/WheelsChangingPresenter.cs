using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class WheelsChangingPresenter: IDisposable
    {
        private readonly IHangarUseCase _hangarUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public event Action OnStartUseCase = delegate { };
        public event Action OnFinishUseCase = delegate { };
        
        public event Action<bool> OnSetAvailable = delegate { };
        public event Action<bool> OnBuyAvailable = delegate { };
        
        public WheelsChangingPresenter(IHangarUseCase hangarUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnStartUseCase += Start;
            _wheelsChangingUseCase.OnFinishUseCase += Finish;
            
            _cancellationTokenSource = new CancellationTokenSource();
        }


        public void Dispose()
        {
            _wheelsChangingUseCase.OnStartUseCase -= Start;
            _wheelsChangingUseCase.OnFinishUseCase -= Finish;
            _cancellationTokenSource.Dispose();
        }

        private void Start() => OnStartUseCase.Invoke();
        private void Finish() => OnFinishUseCase.Invoke();

        public async UniTask<bool> TryOutWheels(WheelsDataView wheelsDataView)
        {
            var wheelsData = new WheelsData { Id = wheelsDataView.Id, Price = wheelsDataView.Price };
            var result = await _wheelsChangingUseCase.TryWheelsOut(wheelsData, _cancellationTokenSource.Token);
            if (!result)
                return false;

            OnSetAvailable.Invoke(await _wheelsChangingUseCase.IsSetAvailable(wheelsData, _cancellationTokenSource.Token));
            OnBuyAvailable.Invoke(await _wheelsChangingUseCase.IsBuyAvailable(wheelsData, _cancellationTokenSource.Token));
            return true;
        }

        public async UniTask<List<WheelsDataView>> GetWheelsDataView(CancellationToken cancellationToken)
        {
            var vehicle = await _hangarUseCase.GetCurrentVehicle(cancellationToken);
            var allWheelsData = await _wheelsChangingUseCase.GetAllWheels(vehicle, cancellationToken);
            var boughtWheelsData = await _wheelsChangingUseCase.GetBoughtWheels(vehicle, cancellationToken);
            var currentWheelsData = await _wheelsChangingUseCase.GetCurrentWheels(vehicle, cancellationToken);
            var wheelsDataViews = new List<WheelsDataView>();

            foreach (var wheelsData in allWheelsData)
            {
                var wheelsDataView = new WheelsDataView
                {
                    Id = wheelsData.Id,
                    Price = wheelsData.Price,
                    Status = ""
                };

                if (boughtWheelsData.Contains(wheelsData))
                {
                    wheelsDataView.Status = "Bought";
                }

                if (wheelsData.Equals(currentWheelsData))
                    wheelsDataView.Status = "Current";
                
                wheelsDataViews.Add(wheelsDataView);
            }
            
            return wheelsDataViews;
        }
    }
}
