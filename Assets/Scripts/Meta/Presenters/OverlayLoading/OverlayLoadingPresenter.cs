using Meta.UseCases;

namespace Meta.Presenters
{
    public class OverlayLoadingPresenter : IOverlayLoadingPresenter
    {
        private readonly IOverlayLoadingView _overlayLoadingView;

        public OverlayLoadingPresenter(IOverlayLoadingView overlayLoadingView)
        {
            _overlayLoadingView = overlayLoadingView;
        }
    
        public void Dispose()
        {
            // TODO release managed resources here
        }

        public void ShowOverlayLoading() => _overlayLoadingView.ShowOverlayLoading();
        public void HideOverlayLoading() => _overlayLoadingView.HideOverlayLoading();
    }
}