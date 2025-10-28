using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class AuthenticatorPresenter: IAuthenticatorPresenter, IDisposable
    {
        public event Action OnAuthenticate = delegate { };

        private readonly IAuthenticatorView _authenticatorView;

        public AuthenticatorPresenter(IAuthenticatorView authenticatorView)
        {
            _authenticatorView = authenticatorView;
            _authenticatorView.OnAuthenticate += InvokeOnAuthenticate;
        }

        public void Dispose()
        {
            _authenticatorView.OnAuthenticate -= InvokeOnAuthenticate;
        }

        private void InvokeOnAuthenticate() => OnAuthenticate.Invoke();

        public void ShowReadyState() => _authenticatorView.ShowReadyState();

        public void ShowInProgressState() => _authenticatorView.ShowInProgressState();

        public void ShowAuthResponse(bool isSuccess) => _authenticatorView.ShowAuthResponse(isSuccess);
    }
}
