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
        private readonly ISpecialOffersCongratsFactory _specialOfferCongratsFactory;
        private readonly ISpecialOffersService _specialOffersService;
        private readonly IOverlayLoadingFactory _overlayLoadingFactory;
        
        private ISpecialOffersPresenter _specialOffersPresenter;
        private ISpecialOfferPresenter _specialOfferPresenter;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        public SpecialOffersUseCase(
            ISpecialOffersFactory specialOffersFactory,
            ISpecialOffersService specialOffersService,
            ISpecialOffersCongratsFactory specialOfferCongratsFactory,
            ISpecialOfferFactory specialOfferFactory,
            IOverlayLoadingFactory overlayLoadingFactory)
        {
            _specialOffersFactory = specialOffersFactory;
            _specialOffersService = specialOffersService;
            _specialOfferCongratsFactory = specialOfferCongratsFactory;
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
            
            if(_specialOfferPresenter != null)
                _specialOfferFactory.DestroySpecialOfferPresenter(_specialOfferPresenter);
        }

        private void ShowSpecialOfferPopup(string specialOfferId) => SpecialOfferPopupAsync(specialOfferId).Forget();

        private async UniTask<bool> SpecialOfferPopupAsync(string specialOfferId)
        {
            var overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
            _specialOfferPresenter =
                await _specialOfferFactory.GetSpecialOfferPresenter(specialOfferId, _cancellationTokenSource.Token);
            _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);

            var userChoice = await _specialOfferPresenter.GetUserChoice();
            
            overlay = await _overlayLoadingFactory.GetOverlayLoadingPresenter(_cancellationTokenSource.Token);
            _specialOfferFactory.DestroySpecialOfferPresenter(_specialOfferPresenter);
            _specialOfferPresenter = null;

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

                    var congrats = await _specialOfferCongratsFactory.GetInfoPopupPresenter(specialOfferId, _cancellationTokenSource.Token);
                    _overlayLoadingFactory.DestroyOverlayLoadingPresenter(overlay);
                    await congrats.GetClick();
                    _specialOfferCongratsFactory.DestroyInfoPopupPresenter(congrats);
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
