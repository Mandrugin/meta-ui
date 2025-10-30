using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOffersService
    {
        UniTask<List<SpecialOfferData>> GetAvailableSpecialOffers();
        UniTask<bool> UseOffer(string offerId);
    }
}