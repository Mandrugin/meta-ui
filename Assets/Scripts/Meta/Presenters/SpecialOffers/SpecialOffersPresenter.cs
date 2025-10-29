using System;
using System.Collections.Generic;
using System.Linq;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOffersPresenter: ISpecialOffersPresenter
    {
        public event Action<string> OnClickSpecialOffer = delegate { };
        
        private readonly ISpecialOffersView _specialOffersView;

        public SpecialOffersPresenter(ISpecialOffersView specialOffersView)
        {
            _specialOffersView = specialOffersView;
            _specialOffersView.OnClickSpecialOffer += OnClickSpecialOfferInvocator;
        }

        public void Dispose()
        {
            _specialOffersView.OnClickSpecialOffer -= OnClickSpecialOfferInvocator;
            _specialOffersView.Dispose();
        }

        private void OnClickSpecialOfferInvocator(string specialOfferId)
            => OnClickSpecialOffer.Invoke(specialOfferId);

        public void AddSpecialOffer(SpecialOfferData specialOfferId)
        {
            _specialOffersView.AddSpecialOffer(specialOfferId.ToDataView());
        }

        public void RemoveSpecialOffer(SpecialOfferData specialOfferId)
        {
            _specialOffersView.RemoveSpecialOffer(specialOfferId.ToDataView());
        }

        public void AddSpecialOffers(List<SpecialOfferData> specialOfferIds)
        {
            _specialOffersView.AddSpecialOffers(specialOfferIds.Select(x => x.ToDataView()).ToList());
        }

        public void RemoveSpecialOffers(List<SpecialOfferData> specialOfferIds)
        {
            _specialOffersView.RemoveSpecialOffers(specialOfferIds.Select(x => x.ToDataView()).ToList());
        }
    }
}
