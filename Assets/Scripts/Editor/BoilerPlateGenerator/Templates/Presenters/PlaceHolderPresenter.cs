using Meta.UseCases;

namespace Meta.Presenters
{
    public class PlaceHolderPresenter: IPlaceHolderPresenter
    {
        private readonly IPlaceHolderView _placeHolderView;

        public PlaceHolderPresenter(IPlaceHolderView placeHolderView)
        {
            _placeHolderView = placeHolderView;
        }

        public void Dispose()
        {
            _placeHolderView.Dispose();
        }
    }
}
