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

            ShowCarousel();
        }

        public void Dispose()
        {
            _specialOffersPresenter.OnClickSpecialOffer -= ShowSpecialOfferPopup;
            _specialOffersFactory.DestroySpecialOffersPresenter(_specialOffersPresenter);
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        private void ShowSpecialOfferPopup(string specialOfferId) => SpecialOfferPopupAsync(specialOfferId).Forget();

        private async UniTask<bool> SpecialOfferPopupAsync(string specialOfferId)
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
                    Debug.Log($"offer with Id {specialOfferId} is used");
                    
                    _specialOffersPresenter.RemoveSpecialOffer(new SpecialOfferData
                    {
                        Id = specialOfferId,
                    });

                    return true;
                }
            }

            return false;
        }

        private void ShowCarousel()
        {
            ShowCarouselAsync().Forget();
        }

        private async UniTask ShowCarouselAsync()
        {
            var availableSpecialOffers = await _specialOffersService.GetAvailableSpecialOffers();

            foreach (var availableSpecialOffer in availableSpecialOffers)
            {
                if (await SpecialOfferPopupAsync(availableSpecialOffer.Id))
                    break;
            }
        }
    }
}
