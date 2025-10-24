using System;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class HangarPresenter : IDisposable, IHangarPresenter
    {
        public event Action OnShowWheelsChanging = delegate { };
        public event Action OnHideWheelsChanging = delegate { };
        
        private readonly IHangarView _hangarView;

        public HangarPresenter(IHangarView hangarView)
        {
            _hangarView = hangarView;
            _hangarView.OnShowWheelsChanging += InvokeOnShowWheelsChanging;
            _hangarView.OnHideWheelsChanging += InvokeOnHideWheelsChanging;
        }

        public void Dispose()
        {
            _hangarView.OnShowWheelsChanging -= InvokeOnShowWheelsChanging;
            _hangarView.OnHideWheelsChanging -= InvokeOnHideWheelsChanging;
        }

        private void InvokeOnShowWheelsChanging() => OnShowWheelsChanging.Invoke();
        private void InvokeOnHideWheelsChanging() => OnHideWheelsChanging.Invoke();

        public void ChangeHard(long hard) => _hangarView.ChangeHard(hard);
        public void ChangeSoft(long soft) => _hangarView.ChangeSoft(soft);
    }
}
