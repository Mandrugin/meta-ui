using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOffersCongratsFactory : IDisposable
    {
        public UniTask<ISpecialOfferCongratsPresenter> GetInfoPopupPresenter(string specialOfferId, CancellationToken cancellationToken);
        public void DestroyInfoPopupPresenter(ISpecialOfferCongratsPresenter specialOfferCongratsPresenter);
    }
}
