using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Services
{
    public class TestSpecialOffersService : ISpecialOffersService
    {
        private readonly SpecialOffersDataConfig _specialOffersDataConfig;

        public TestSpecialOffersService(SpecialOffersDataConfig specialOffersDataConfig)
        {
            _specialOffersDataConfig = specialOffersDataConfig;
        }

        public async UniTask<List<SpecialOfferData>> GetAvailableSpecialOffers()
        {
            await UniTask.WaitForSeconds(1);
            return _specialOffersDataConfig.specialOffers.Select(x => new SpecialOfferData
            {
                Id = x.id,
                TimeStamp = 0
            }).ToList();
        }
    }
}