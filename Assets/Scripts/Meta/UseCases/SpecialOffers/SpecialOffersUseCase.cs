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
        private readonly IOverlayLoadingFactory _overlayLoadingFactory;
        
        private ISpecialOffersPresenter _specialOffersPresenter;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public SpecialOffersUseCase(
            ISpecialOffersFactory specialOffersFactory,
            ISpecialOffersService specialOffersService,
            ISpecialOfferFactory specialOfferFactory,
            IOverlayLoadingFactory overlayLoadingFactory)
        {
            _specialOffersFactory = specialOffersFactory;
            _specialOffersService = specialOffersService;
            _specialOfferFactory = specialOfferFactory;
            _overlayLoadingFactory = overlayLoadingFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new())
        {
            var overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
            _specialOffersPresenter = await _specialOffersFactory.GetSpecialOffersPresenter(cancellation);

            var availableSpecialOffers = await _specialOffersService.GetAvailableSpecialOffers();
            
            _specialOffersPresenter.AddSpecialOffers(availableSpecialOffers);
            _specialOffersPresenter.OnClickSpecialOffer += ShowSpecialOfferPopup;
            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);

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
            var overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
            var specialOfferPresenter =
                await _specialOfferFactory.GetSpecialOfferPresenter(specialOfferId, _cancellationTokenSource.Token);
            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);

            var userChoice = await specialOfferPresenter.GetUserChoice();
            
            overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
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

                    _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);
                    return true;
                }
            }

            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);
            return false;
        }

        private void ShowCarousel()
        {
            ShowCarouselAsync().Forget();
        }

        private async UniTask ShowCarouselAsync()
        {
            var overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
            var availableSpecialOffers = await _specialOffersService.GetAvailableSpecialOffers();
            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);

            foreach (var availableSpecialOffer in availableSpecialOffers)
            {
                if (await SpecialOfferPopupAsync(availableSpecialOffer.Id))
                    break;
            }
        }
    }
}
