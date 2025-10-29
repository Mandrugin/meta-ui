using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Meta.UseCases
{
    public class SpecialOffersUseCase: IAsyncStartable, IDisposable
    {
        private readonly ISpecialOffersFactory  _specialOffersFactory;
        private readonly ISpecialOffersService _specialOffersService;
        
        private ISpecialOffersPresenter _specialOffersPresenter;
        
        public SpecialOffersUseCase(ISpecialOffersFactory specialOffersFactory, ISpecialOffersService specialOffersService)
        {
            _specialOffersFactory = specialOffersFactory;
            _specialOffersService = specialOffersService;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _specialOffersPresenter = await _specialOffersFactory.GetSpecialOffersPresenter(cancellation);

            var availableSpecialOffers = await _specialOffersService.GetAvailableSpecialOffers();
            
            _specialOffersPresenter.AddSpecialOffers(availableSpecialOffers);
        }

        public void Dispose()
        {
            _specialOffersFactory.DestroySpecialOffersPresenter(_specialOffersPresenter);
        }
    }
}
