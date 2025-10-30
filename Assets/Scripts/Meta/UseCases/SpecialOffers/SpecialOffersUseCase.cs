using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace Meta.UseCases
{
    public class SpecialOffersUseCase: IAsyncStartable, IDisposable
    {
        private readonly ISpecialOffersFactory  _specialOffersFactory;
        private readonly ISpecialOfferFactory _specialOfferFactory;
        private readonly ISpecialOffersService _specialOffersService;
        
        private ISpecialOffersPresenter _specialOffersPresenter;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public SpecialOffersUseCase(
            ISpecialOffersFactory specialOffersFactory,
            ISpecialOffersService specialOffersService,
            ISpecialOfferFactory specialOfferFactory)
        {
            _specialOffersFactory = specialOffersFactory;
            _specialOffersService = specialOffersService;
            _specialOfferFactory = specialOfferFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            _specialOffersPresenter = await _specialOffersFactory.GetSpecialOffersPresenter(cancellation);

            var availableSpecialOffers = await _specialOffersService.GetAvailableSpecialOffers();
            
            _specialOffersPresenter.AddSpecialOffers(availableSpecialOffers);
            _specialOffersPresenter.OnClickSpecialOffer += ShowSpecialOfferPopup;
        }

        public void Dispose()
        {
            _specialOffersPresenter.OnClickSpecialOffer -= ShowSpecialOfferPopup;
            _specialOffersFactory.DestroySpecialOffersPresenter(_specialOffersPresenter);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void ShowSpecialOfferPopup(string specialOfferId) => SpecialOfferPopupAsync(specialOfferId).Forget();

        private async UniTask SpecialOfferPopupAsync(string specialOfferId)
        {
            var specialOfferPresenter =
                await _specialOfferFactory.GetSpecialOfferPresenter(specialOfferId, _cancellationTokenSource.Token);

            var userChoice = await specialOfferPresenter.GetUserChoice();
            
            _specialOfferFactory.DestroySpecialOfferPresenter(specialOfferPresenter);

            if (userChoice)
            {
                var useOffer = await _specialOffersService.UseOffer(specialOfferId);
                
                if(useOffer)
                {
                    Debug.Log($"{specialOfferId} used offer");
                    
                    _specialOffersPresenter.RemoveSpecialOffer(new SpecialOfferData
                    {
                        Id = specialOfferId,
                    });
                }
            }

        }
    }
}
