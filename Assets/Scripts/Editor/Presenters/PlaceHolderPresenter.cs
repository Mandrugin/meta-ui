using Meta.UseCases;

namespace Meta.Presenters
{
    public class PlaceHolderPresenter: IPlaceHolderPresenter
    {
        private readonly IPlaceHolderView placeHolderView;

        public PlaceHolderPresenter(IPlaceHolderView placeHolderView)
        {
            this.placeHolderView = placeHolderView;
        }

        public void Dispose()
        {
        }
    }
}