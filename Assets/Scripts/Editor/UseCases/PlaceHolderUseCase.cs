using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Meta.UseCases
{
    public class PlaceHolderUseCase: IAsyncStartable, IDisposable
    {
        private readonly IPlaceHolderFactory  _placeHolderFactory;
        
        private IPlaceHolderPresenter _placeHolderPresenter;
        
        public PlaceHolderUseCase(IPlaceHolderFactory placeHolderFactory)
        {
            _placeHolderFactory = placeHolderFactory;
        }

        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            _placeHolderPresenter = await _placeHolderFactory.GetPlaceHolderPresenter(cancellation);
        }

        public void Dispose()
        {
            
        }
    }
}