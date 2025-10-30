using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOfferPresenter: ISpecialOfferPresenter
    {
        private readonly ISpecialOfferView _SpecialOfferView;

        public SpecialOfferPresenter(ISpecialOfferView SpecialOfferView)
        {
            _SpecialOfferView = SpecialOfferView;
        }

        public void Dispose()
        {
            _SpecialOfferView.Dispose();
        }
    }
}
