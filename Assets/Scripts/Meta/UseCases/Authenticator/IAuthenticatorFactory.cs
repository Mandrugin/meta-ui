using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IAuthenticatorFactory : IDisposable
    {
        public UniTask<IAuthenticatorPresenter> GetAuthenticatorPresenter(CancellationToken cancellationToken);
        public void DestroyAuthenticatorPresenter(IAuthenticatorPresenter authenticatorPresenter);
    }
}
