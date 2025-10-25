using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IOverlayLoadingFactory
    {
        UniTask<IOverlayLoadingPresenter> GetOverlayLoadingPresenter(CancellationToken cancellationToken);
        void DestroyOverlayLoadingPresenter();
    }
}