using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Presenters
{
    [Preserve]
    public class HangarPresenter : IDisposable
    {
        private readonly IHangarUseCase _hangarUseCase;
        private readonly IWheelsChangingUseCase _wheelsChangingUseCase;

        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };

        public HangarPresenter(IHangarUseCase hangarUseCase, IWheelsChangingUseCase wheelsChangingUseCase)
        {
            _hangarUseCase = hangarUseCase;
            _wheelsChangingUseCase = wheelsChangingUseCase;

            _hangarUseCase.OnHardChanged += OnOnHardChanged;
            _hangarUseCase.OnSoftChanged += OnOnSoftChanged;
        }

        public void Dispose()
        {
            _hangarUseCase.OnHardChanged -= OnOnHardChanged;
            _hangarUseCase.OnSoftChanged -= OnOnSoftChanged;
        }

        public void StartWheelsChanging() => _wheelsChangingUseCase.ShowPresenter();
        public void FinishWheelsChanging() => _wheelsChangingUseCase.HidePresenter();
        private void OnOnHardChanged(long hard) => OnHardChanged.Invoke(hard);
        private void OnOnSoftChanged(long soft) => OnSoftChanged.Invoke(soft);

        public async UniTask<long> GetHardBalance(CancellationToken cancellationToken)
        {
            return await _hangarUseCase.GetHardBalance(cancellationToken);
        }

        public async UniTask<long> GetSoftBalance(CancellationToken cancellationToken)
        {
            return await _hangarUseCase.GetSoftBalance(cancellationToken);
        }
    }
}
