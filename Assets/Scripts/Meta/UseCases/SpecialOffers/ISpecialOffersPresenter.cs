using System;
using System.Collections.Generic;
using System.Data;

namespace Meta.UseCases
{
    public interface ISpecialOffersPresenter : IDisposable
    {
        event Action<string> OnClickSpecialOffer;
        void AddSpecialOffer(SpecialOfferData specialOfferId);
        void RemoveSpecialOffer(SpecialOfferData specialOfferId);
        void AddSpecialOffers(List<SpecialOfferData> specialOfferIds);
        void RemoveSpecialOffers(List<SpecialOfferData> specialOfferIds);
    }
}
