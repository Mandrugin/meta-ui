using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOffersCongratsPresenter: ISpecialOfferCongratsPresenter
    {
        private readonly ISpecialOffersCongratsView specialOffersCongratsView;

        public SpecialOffersCongratsPresenter(ISpecialOffersCongratsView specialOffersCongratsView)
        {
            this.specialOffersCongratsView = specialOffersCongratsView;
        }

        public void Dispose()
        {
            specialOffersCongratsView.Dispose();
        }

        public UniTask GetClick()
        {
            return specialOffersCongratsView.GetClick();
        }
    }
}
