using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IOverlayLoadingFactory: IDisposable
    {
        UniTask<IOverlayLoadingPresenter> GetOverlayLoadingPresenter(CancellationToken cancellationToken);
        void DestroyOverlayLoadingPresenter(IOverlayLoadingPresenter overlayLoadingPresenter);
    }
}