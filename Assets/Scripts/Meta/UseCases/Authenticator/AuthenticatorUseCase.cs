using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Services;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Meta.UseCases
{
    public class AuthenticatorUseCase: IAsyncStartable, IDisposable
    {
        private readonly IAuthenticatorFactory  _authenticatorFactory;
        private readonly IAuthenticationService _authenticationService;
        private readonly string _nextSceneName;
        
        private IAuthenticatorPresenter _authenticatorPresenter;
        
        private UniTask _authenticationTask = UniTask.CompletedTask;

        public AuthenticatorUseCase(
            IAuthenticatorFactory authenticatorFactory,
            IAuthenticationService authenticationService,
            string nextSceneName)
        {
            _authenticatorFactory = authenticatorFactory;
            _authenticationService = authenticationService;
            _nextSceneName = nextSceneName;
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
                SceneManager.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
            }
        }
    }
}