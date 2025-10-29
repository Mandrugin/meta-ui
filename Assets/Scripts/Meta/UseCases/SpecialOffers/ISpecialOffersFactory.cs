using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOffersFactory : IDisposable
    {
        public UniTask<ISpecialOffersPresenter> GetSpecialOffersPresenter(CancellationToken cancellationToken);
        public void DestroySpecialOffersPresenter(ISpecialOffersPresenter specialOffersPresenter);
    }
}
