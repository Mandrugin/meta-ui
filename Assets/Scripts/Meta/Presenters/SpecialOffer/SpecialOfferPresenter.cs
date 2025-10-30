using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Presenters
{
    public class SpecialOfferPresenter: ISpecialOfferPresenter
    {
        private readonly ISpecialOfferView _specialOfferView;

        public SpecialOfferPresenter(ISpecialOfferView specialOfferView)
        {
            _specialOfferView = specialOfferView;
        }

        public void Dispose()
        {
            _specialOfferView.Dispose();
        }

        public UniTask<bool> GetUserChoice()
        {
            return _specialOfferView.GetUserChoice();
        }
    }
}
