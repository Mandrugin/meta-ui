using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class WheelsChangingPresenter : IDisposable, IWheelsChangingPresenter
    {
        private readonly IWheelsChangingView _wheelsChangingView;

        public event Action OnSetWheels = delegate { };
        public event Action OnBuyWheels = delegate { };
        public event Action<WheelsData> OnWheelsTriedOut = delegate { };

        public WheelsChangingPresenter(IWheelsChangingView wheelsChangingView)
        {
            _wheelsChangingView = wheelsChangingView;
            _wheelsChangingView.OnSetWheels += OnSetWheelsInvocator;
            _wheelsChangingView.OnBuyWheels += OnBuyWheelsInvocator;
            _wheelsChangingView.OnWheelsTriedOut += OnWheelsTriedOutInvocator;
        }

        public void Dispose()
        {
            _wheelsChangingView.OnSetWheels -= OnSetWheelsInvocator;
            _wheelsChangingView.OnBuyWheels -= OnBuyWheelsInvocator;
            _wheelsChangingView.OnWheelsTriedOut -= OnWheelsTriedOutInvocator;
        }

        private void OnSetWheelsInvocator() => OnSetWheels.Invoke();

        private void OnBuyWheelsInvocator() => OnBuyWheels.Invoke();

        private void OnWheelsTriedOutInvocator(WheelsDataView wheelsDataView) =>
            OnWheelsTriedOut.Invoke(wheelsDataView.ToWheelsData());

        public void SetSetAvailable(bool isAvailable)
        {
            _wheelsChangingView.SetSetAvailable(isAvailable);
        }

        public void SetBuyAvailable(bool isAvailable)
        {
            _wheelsChangingView.SetBuyAvailable(isAvailable);
        }

        public void ChangeWheelsList(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            var wheelsDataViews = ConvertToWheelsDataViews(allWheelsData, boughtWheelsData, setWheelsData);
            _wheelsChangingView.ChangeWheelsList(wheelsDataViews);

            foreach (var wheelsDataView in wheelsDataViews)
            {
                if (wheelsDataView.Id != setWheelsData.Id)
                    continue;

                _wheelsChangingView.SetCurrentWheels(wheelsDataView);
                return;
            }
        }

        private static List<WheelsDataView> ConvertToWheelsDataViews(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData)
        {
            var wheelsDataViews = new List<WheelsDataView>();

            foreach (var wheelsData in allWheelsData)
            {
                var wheelsDataView = wheelsData.ToWheelsDataView();

                wheelsDataView.IsBought = boughtWheelsData.Contains(wheelsData);

                wheelsDataView.IsSet = wheelsData.Equals(setWheelsData);
                
                wheelsDataViews.Add(wheelsDataView);
            }

            return wheelsDataViews;
        }
    }
}
