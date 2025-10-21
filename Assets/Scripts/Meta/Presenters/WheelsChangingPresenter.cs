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
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };
        
        public event Action<bool> OnSetAvailable = delegate { };
        public event Action<bool> OnBuyAvailable = delegate { };
        
        public event Action<List<WheelsDataView>> OnWheelsListChanged = delegate { };
        public event Action<WheelsDataView> OnSetActiveWheels = delegate { };
        
        public WheelsChangingPresenter(IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _wheelsChangingUseCase = wheelsChangingUseCase;
            
            _wheelsChangingUseCase.OnWheelsListChanged += OnOnWheelsListChanged;
            _wheelsChangingUseCase.OnCurrentWheelsChanged += OnOnCurrentWheelsChanged;
            _wheelsChangingUseCase.OnShowPresenter += ShowPresenter;
            _wheelsChangingUseCase.OnHidePresenter += HidePresenter;
            _wheelsChangingUseCase.OnWheelsSet += OnWheelsSet;
            _wheelsChangingUseCase.OnWheelsBought += OnWheelsBought;
            _wheelsChangingUseCase.OnSetAvailable += OnOnSetAvailable;
            _wheelsChangingUseCase.OnBuyAvailable += OnOnBuyAvailable;
            
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnWheelsListChanged -= OnOnWheelsListChanged;
            _wheelsChangingUseCase.OnShowPresenter -= ShowPresenter;
            _wheelsChangingUseCase.OnHidePresenter -= HidePresenter;
            _wheelsChangingUseCase.OnWheelsSet -= OnWheelsSet;
            _wheelsChangingUseCase.OnWheelsBought -= OnWheelsBought;
            _wheelsChangingUseCase.OnSetAvailable -= OnOnSetAvailable;
            _wheelsChangingUseCase.OnBuyAvailable -= OnOnBuyAvailable;
            
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void OnOnCurrentWheelsChanged(WheelsData wheelsData)
        {
            OnSetActiveWheels.Invoke(wheelsData.ToWheelsDataView());
        }

        private void OnWheelsSet(WheelsData wheelsData)
        {
            OnSetAvailable.Invoke(false);
        }

        private void OnWheelsBought(WheelsData wheelsData)
        {
            OnBuyAvailable.Invoke(false);
        }

        private void OnOnSetAvailable(bool isAvailable)
        {
            OnSetAvailable.Invoke(isAvailable);
        }

        private void OnOnBuyAvailable(bool isAvailable)
        {
            OnBuyAvailable.Invoke(isAvailable);
        }

        private void OnOnWheelsListChanged(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            var wheelsDataViews = ConvertToWheelsDataViews(allWheelsData, boughtWheelsData, setWheelsData);
            OnWheelsListChanged.Invoke(wheelsDataViews);

            foreach (var wheelsDataView in wheelsDataViews)
            {
                if (wheelsDataView.Id != setWheelsData.Id)
                    continue;

                OnSetActiveWheels.Invoke(wheelsDataView);
                return;
            }
        }

        public async UniTask<bool> SetWheels()
        {
            return await _wheelsChangingUseCase.SetWheels(_cancellationTokenSource.Token);
        }

        public async UniTask<bool> BuyWheels()
        {
            return await _wheelsChangingUseCase.BuyWheels(_cancellationTokenSource.Token);
        }

        private void ShowPresenter() => OnShowPresenter.Invoke();
        private void HidePresenter() => OnHidePresenter.Invoke();

        public async UniTask<bool> TryOutWheels(WheelsDataView wheelsDataView, CancellationToken cancellationToken)
        {
            var wheelsData = new WheelsData { Id = wheelsDataView.Id, Price = wheelsDataView.Price };
            var result = await _wheelsChangingUseCase.TryWheelsOut(wheelsData, cancellationToken);
            if (!result)
                return false;
            return true;
        }

        public async UniTask UpdateWheelsData(CancellationToken cancellationToken)
        {
            await _wheelsChangingUseCase.UpdateWheelsData(cancellationToken);
        }

        private static List<WheelsDataView> ConvertToWheelsDataViews(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            var wheelsDataViews = new List<WheelsDataView>();

            foreach (var wheelsData in allWheelsData)
            {
                var wheelsDataView = wheelsData.ToWheelsDataView();

                if (boughtWheelsData.Contains(wheelsData))
                    wheelsDataView.Status = "Bought";

                if (wheelsData.Equals(setWheelsData))
                    wheelsDataView.Status = "Current";
                
                wheelsDataViews.Add(wheelsDataView);
            }

            return wheelsDataViews;
        }
    }
}
