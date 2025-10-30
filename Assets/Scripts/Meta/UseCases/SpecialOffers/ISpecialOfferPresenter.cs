using System;

namespace Meta.UseCases
{
    public interface ISpecialOfferPresenter : IDisposable
    {
        event Action<string> OnGetSpecialOffer;
        event Action DismissSpecialOffer;
        public void Init(string specialOfferId);
    }
}
