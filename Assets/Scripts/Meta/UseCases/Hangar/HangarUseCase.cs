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
        private readonly UseCaseMediator _useCaseMediator;
        public event Action OnShowPresenter = delegate { };
        public event Action OnHidePresenter = delegate { };
        public event Action<long> OnHardChanged = delegate { };
        public event Action<long> OnSoftChanged = delegate { };
        
        private Vehicle _currentVehicle;

        public HangarUseCase(IHangarGateway hangarGateway, UseCaseMediator useCaseMediator)
        {
            _hangarGateway = hangarGateway;
            _useCaseMediator = useCaseMediator;
            _hangarGateway.OnHardChanged += OnOnHardChanged;
            _hangarGateway.OnSoftChanged += OnOnSoftChanged;
        }

        public void Dispose()
        {
        }

        private void OnOnSoftChanged(long soft)
        {
            OnSoftChanged.Invoke(soft);
        }

        private void OnOnHardChanged(long hard)
        {
            OnHardChanged.Invoke(hard);
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

        public void StartWheelsChanging() => _useCaseMediator.ShowWheelsChanging();
        public void FinishWheelsChanging() => _useCaseMediator.HideWheelsChanging();


    }
}
