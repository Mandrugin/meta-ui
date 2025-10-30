using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Meta.DataConfigs;
using Meta.UseCases;

namespace Meta.Services
{
    public class TestSpecialOffersService : ISpecialOffersService
    {
        private readonly SpecialOffersDataConfig _specialOffersDataConfig;
        private readonly ProfileDataConfig _profileDataConfig;

        public TestSpecialOffersService(SpecialOffersDataConfig specialOffersDataConfig, ProfileDataConfig profileDataConfig)
        {
            _specialOffersDataConfig = specialOffersDataConfig;
            _profileDataConfig = profileDataConfig;
        }

        public async UniTask<List<SpecialOfferData>> GetAvailableSpecialOffers()
        {
            await UniTask.WaitForSeconds(1);
            return _specialOffersDataConfig.specialOffers
                .Where(x => !_profileDataConfig.usedOffers.Contains(x.id))
                .Select(x => new SpecialOfferData
            {
                Id = x.id,
                TimeStamp = 0
            }).ToList();
        }

        public async UniTask<bool> UseOffer(string offerId)
        {
            _profileDataConfig.usedOffers.Add(offerId);
            await UniTask.WaitForSeconds(1);
            return true;
        }
    }
}