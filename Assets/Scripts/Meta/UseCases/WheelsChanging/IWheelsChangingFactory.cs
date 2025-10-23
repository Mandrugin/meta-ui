using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IWheelsChangingFactory : IDisposable
    { 
        UniTask<IWheelsChangingPresenter> GetWheelsChangingPresenter(CancellationToken cancellationToken);
        void DestroyWheelsChangingPresenter();
    }
}