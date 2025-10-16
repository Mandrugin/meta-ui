using System;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class HangarPresenter : IDisposable
    {
        private readonly IHangarUseCase _hangarUseCase;

        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };

        public HangarPresenter(IHangarUseCase hangarUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _hangarUseCase.OnHardChanged += OnOnHardChanged;
            _hangarUseCase.OnSoftChanged += OnOnSoftChanged;
        }

        public void StartWheelsChanging() => _hangarUseCase.StartWheelsChanging();

        public void FinishWheelsChanging() => _hangarUseCase.FinishWheelsChanging();

        private void OnOnHardChanged(long hard) => OnHardChanged.Invoke(hard);
        private void OnOnSoftChanged(long soft) => OnSoftChanged.Invoke(soft);

        public void Dispose()
        {
            _hangarUseCase.OnHardChanged -= OnOnHardChanged;
            _hangarUseCase.OnSoftChanged -= OnOnSoftChanged;
        }
    }
}
