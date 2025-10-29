using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IPlaceHolderFactory : IDisposable
    {
        public UniTask<IPlaceHolderPresenter> GetPlaceHolderPresenter(CancellationToken cancellationToken);
        public void DestroyPlaceHolderPresenter(IPlaceHolderPresenter placeHolderPresenter);
    }
}
