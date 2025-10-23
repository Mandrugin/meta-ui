using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Entities;
using UnityEngine.Scripting;
using VContainer.Unity;

namespace Meta.UseCases
{
    [Preserve]
    public class HangarUseCase : IDisposable, IAsyncStartable
    {
        private readonly IHangarGateway _hangarGateway;
        private readonly IHangarFactory _hangarFactory;
        
        private IHangarPresenter _hangarPresenter;
        private Vehicle _currentVehicle;

        public HangarUseCase(IHangarGateway hangarGateway, IHangarFactory hangarFactory)
        {
            _hangarGateway = hangarGateway;
            _hangarFactory = hangarFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _hangarPresenter = await _hangarFactory.GetHangarPresenter(cancellation);
            
            _hangarPresenter.ChangeHard(await _hangarGateway.GetHardBalance(cancellation));
            _hangarPresenter.ChangeSoft(await _hangarGateway.GetSoftBalance(cancellation));

            // _hangarPresenter.OnShowWheelsChanging += _wheelsChangingUseCase.ShowPresenter;
            // _hangarPresenter.OnHideWheelsChanging += _wheelsChangingUseCase.HidePresenter;
            
            _hangarGateway.OnHardChanged += ChangeHard;
            _hangarGateway.OnSoftChanged += ChangeSoft;            
        }

        public void Dispose()
        {
            // _hangarPresenter.OnShowWheelsChanging -= _wheelsChangingUseCase.ShowPresenter;
            // _hangarPresenter.OnHideWheelsChanging -= _wheelsChangingUseCase.HidePresenter;
            
            _hangarGateway.OnHardChanged -= ChangeHard;
            _hangarGateway.OnSoftChanged -= ChangeSoft;
        }

        private void ChangeHard(long hard) => _hangarPresenter.ChangeHard(hard);
        private void ChangeSoft(long soft) => _hangarPresenter.ChangeSoft(soft);
    }
}
