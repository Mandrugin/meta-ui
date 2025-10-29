using System;

namespace Meta.Presenters
{
    public interface IAuthenticatorView: IDisposable
    {
        event Action OnAuthenticate;
        void ShowReadyState();
        void ShowInProgressState();
        void ShowAuthResponse(bool isSuccess);
    }
}
