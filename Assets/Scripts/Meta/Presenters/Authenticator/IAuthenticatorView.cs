using System;
using Cysharp.Threading.Tasks;

namespace Meta.Presenters
{
    public interface IAuthenticatorView
    {
        event Action OnAuthenticate;
        void ShowReadyState();
        void ShowInProgressState();
        void ShowAuthResponse(bool isSuccess);
    }
}
