using System;
using System.Collections.Generic;

namespace Meta.Presenters
{
    public interface ISpecialOffersView: IDisposable
    {
        event Action<string> OnClickSpecialOffer;
        void AddSpecialOffer(SpecialOfferDataView specialOfferId);
        void RemoveSpecialOffer(SpecialOfferDataView specialOfferId);
        void AddSpecialOffers(List<SpecialOfferDataView> specialOfferIds);
        void RemoveSpecialOffers(List<SpecialOfferDataView> specialOfferIds);
    }
}
