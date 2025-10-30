using System;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOfferPresenter: ISpecialOfferPresenter
    {
        public event Action<string> OnGetSpecialOffer = delegate { };
        public event Action DismissSpecialOffer = delegate { };
        
        private readonly ISpecialOfferView _specialOfferView;

        public SpecialOfferPresenter(ISpecialOfferView specialOfferView)
        {
            _specialOfferView = specialOfferView;
        }

        public void Dispose()
        {
            _specialOfferView.Dispose();
        }

        public void Init(string specialOfferId)
        {
            // ...
        }
    }
}
