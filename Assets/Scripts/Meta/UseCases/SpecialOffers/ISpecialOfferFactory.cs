using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOfferFactory : IDisposable
    {
        public UniTask<ISpecialOfferPresenter> GetSpecialOfferPresenter(CancellationToken cancellationToken);
        public void DestroySpecialOfferPresenter(ISpecialOfferPresenter SpecialOfferPresenter);
    }
}
