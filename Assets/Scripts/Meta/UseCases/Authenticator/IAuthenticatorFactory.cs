using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases.Authenticator
{
    public interface IAuthenticatorFactory
    {
        public UniTask<IAuthenticatorPresenter> GetAuthenticatorPresenter(CancellationToken cancellationToken);
        public void DestroyAuthenticatorPresenter();
    }
}
