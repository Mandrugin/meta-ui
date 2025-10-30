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
        private readonly IAuthenticationService _authenticationService;
        private readonly GameObject _hangarScopePrefab;
        private readonly GameObject _specialOffersScopePrefab;
        
        private GameObject _hangarScope;
        private GameObject _specialOffersScope;
        private IAuthenticatorPresenter _authenticatorPresenter;
        
        private UniTask _authenticationTask = UniTask.CompletedTask;

        public AuthenticatorUseCase(
            IAuthenticatorFactory authenticatorFactory,
            IAuthenticationService authenticationService,
            [Key(ScopeKeys.HangarScope)] GameObject hangarScopePrefab,
            [Key(ScopeKeys.SpecialOffersScope)] GameObject specialOffersScopePrefab)
        {
            _authenticatorFactory = authenticatorFactory;
            _authenticationService = authenticationService;
            _hangarScopePrefab = hangarScopePrefab;
            _specialOffersScopePrefab = specialOffersScopePrefab;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _authenticatorPresenter = await _authenticatorFactory.GetAuthenticatorPresenter(cancellation);
            _authenticatorPresenter.ShowInProgressState();

            await _authenticationService.InitializeAsync();
            
            if(_authenticationService.IsAuthenticated)
                MoveOn();
            
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
            var authResponse = await _authenticationService.Authenticate();
            _authenticatorPresenter.ShowAuthResponse(authResponse);

            if (authResponse)
            {
                await UniTask.WaitForSeconds(1);
                MoveOn();
            }
        }

        private void MoveOn()
        {
            _authenticatorFactory.DestroyAuthenticatorPresenter(_authenticatorPresenter);
            _authenticatorPresenter = null;
            _hangarScope = Object.Instantiate(_hangarScopePrefab);
            _specialOffersScope = Object.Instantiate(_specialOffersScopePrefab);            
        }
    }
}