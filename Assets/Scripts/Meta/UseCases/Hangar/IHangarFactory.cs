using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarFactory : IDisposable
    {
        UniTask<IHangarPresenter> GetHangarPresenter(CancellationToken cancellationToken);
        void DestroyHangarPresenter();
    }
}
