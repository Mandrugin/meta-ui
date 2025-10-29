using System;

namespace Meta.UseCases
{
    public interface IAuthenticatorPresenter : IDisposable
    {
        event Action OnAuthenticate;
        void ShowReadyState();
        void ShowInProgressState();
        void ShowAuthResponse(bool isSuccess);
    }
}