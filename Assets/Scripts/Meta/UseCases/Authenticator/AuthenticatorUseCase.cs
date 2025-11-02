using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Meta.UseCases
{
    public class AuthenticatorUseCase: IAsyncStartable, IDisposable
    {
        private readonly IAuthenticatorFactory  _authenticatorFactory;
        private readonly IAuthenticatorService authenticatorService;
        private readonly GameObject _hangarScopePrefab;
        private readonly GameObject _specialOffersScopePrefab;
        
        private GameObject _hangarScope;
        private GameObject _specialOffersScope;
        private IAuthenticatorPresenter _authenticatorPresenter;
        
        private UniTask _authenticationTask = UniTask.CompletedTask;

        public AuthenticatorUseCase(
            IAuthenticatorFactory authenticatorFactory,
            IAuthenticatorService authenticatorService,
            [Key(ScopeKeys.HangarScope)] GameObject hangarScopePrefab,
            [Key(ScopeKeys.SpecialOffersScope)] GameObject specialOffersScopePrefab)
        {
            _authenticatorFactory = authenticatorFactory;
            this.authenticatorService = authenticatorService;
            _hangarScopePrefab = hangarScopePrefab;
            _specialOffersScopePrefab = specialOffersScopePrefab;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            await authenticatorService.InitializeAsync();
            
            if(authenticatorService.IsAuthenticated)
            {
                MoveOn();
                return;
            }

            _authenticatorPresenter = await _authenticatorFactory.GetAuthenticatorPresenter(cancellation);
            _authenticatorPresenter.ShowReadyState();
            _authenticatorPresenter.OnAuthenticate += Authenticate;
        }

        public void Dispose()
        {
            _authenticatorFactory.DestroyAuthenticatorPresenter(_authenticatorPresenter);
        }

        private void Authenticate()
        {
            if (_authenticationTask.Status == UniTaskStatus.Pending)
                return;
            
            _authenticationTask = AuthenticateAsync();
        }

        private async UniTask AuthenticateAsync()
        {
            _authenticatorPresenter.ShowInProgressState();
            var authResponse = await authenticatorService.Authenticate();
            _authenticatorPresenter.ShowAuthResponse(authResponse);

            if (authResponse)
            {
                await UniTask.WaitForSeconds(1);
                _authenticatorFactory.DestroyAuthenticatorPresenter(_authenticatorPresenter);
                _authenticatorPresenter = null;
                MoveOn();
            }
        }

        private void MoveOn()
        {
            _hangarScope = Object.Instantiate(_hangarScopePrefab);
            _specialOffersScope = Object.Instantiate(_specialOffersScopePrefab);            
        }
    }
}