using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Services;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Meta.UseCases
{
    public class AuthenticatorUseCase: IAsyncStartable, IDisposable
    {
        private readonly IAuthenticatorFactory  _authenticatorFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly GameObject _hangarScopePrefab;
        
        private GameObject _hangarScope;
        private IAuthenticatorPresenter _authenticatorPresenter;
        
        private UniTask _authenticationTask = UniTask.CompletedTask;

        public AuthenticatorUseCase(
            IAuthenticatorFactory authenticatorFactory,
            IAuthenticationService authenticationService,
            GameObject hangarScopePrefab)
        {
            _authenticatorFactory = authenticatorFactory;
            _authenticationService = authenticationService;
            _hangarScopePrefab = hangarScopePrefab;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _authenticatorPresenter = await _authenticatorFactory.GetAuthenticatorPresenter(cancellation);
            
            _authenticatorPresenter.ShowReadyState();
            _authenticatorPresenter.OnAuthenticate += Authenticate;
        }

        public void Dispose()
        {
            
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
                _authenticatorFactory.DestroyAuthenticatorPresenter(_authenticatorPresenter);
                _hangarScope = Object.Instantiate(_hangarScopePrefab);
            }
        }
    }
}