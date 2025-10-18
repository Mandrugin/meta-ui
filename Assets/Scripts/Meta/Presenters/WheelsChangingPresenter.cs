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
        
        public event Action<List<WheelsDataView>> OnWheelsListChanged = delegate { };
        public event Action<WheelsDataView> OnSetActiveWheels = delegate { };
        
        public WheelsChangingPresenter(IHangarUseCase hangarUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;
            _wheelsChangingUseCase.OnStartUseCase += Start;
            _wheelsChangingUseCase.OnFinishUseCase += Finish;
            _wheelsChangingUseCase.OnWheelsListChanged += OnOnWheelsListChanged;
            _wheelsChangingUseCase.OnWheelsSet += OnWheelsSet;
            _wheelsChangingUseCase.OnWheelsBought += OnWheelsBought;
            
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _wheelsChangingUseCase.OnStartUseCase -= Start;
            _wheelsChangingUseCase.OnFinishUseCase -= Finish;
            _wheelsChangingUseCase.OnWheelsListChanged -= OnOnWheelsListChanged;
            _wheelsChangingUseCase.OnWheelsSet -= OnWheelsSet;
            _wheelsChangingUseCase.OnWheelsBought -= OnWheelsBought;
            _cancellationTokenSource.Dispose();
        }

        private void OnWheelsSet(WheelsData wheelsData)
        {
            OnSetAvailable.Invoke(false);
        }

        private void OnWheelsBought(WheelsData wheelsData)
        {
            OnBuyAvailable.Invoke(false);
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

        private void Start() => OnStartUseCase.Invoke();
        private void Finish() => OnFinishUseCase.Invoke();

        public async UniTask<bool> TryOutWheels(WheelsDataView wheelsDataView, CancellationToken cancellationToken)
        {
            var wheelsData = new WheelsData { Id = wheelsDataView.Id, Price = wheelsDataView.Price };
            var result = await _wheelsChangingUseCase.TryWheelsOut(wheelsData, cancellationToken);
            if (!result)
                return false;

            OnSetAvailable.Invoke(await _wheelsChangingUseCase.IsSetAvailable(wheelsData, cancellationToken));
            OnBuyAvailable.Invoke(await _wheelsChangingUseCase.IsBuyAvailable(wheelsData, cancellationToken));
            return true;
        }

        public async UniTask UpdateWheelsDataView(CancellationToken cancellationToken)
        {
            await _wheelsChangingUseCase.UpdateWheelsDataView(cancellationToken);
        }

        private static List<WheelsDataView> ConvertToWheelsDataViews(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
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
                    wheelsDataView.Status = "Bought";

                if (wheelsData.Equals(setWheelsData))
                    wheelsDataView.Status = "Current";
                
                wheelsDataViews.Add(wheelsDataView);
            }

            return wheelsDataViews;
        }
    }
}
