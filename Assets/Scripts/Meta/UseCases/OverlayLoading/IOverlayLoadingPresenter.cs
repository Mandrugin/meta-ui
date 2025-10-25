using System;

namespace Meta.UseCases
{
    public interface IOverlayLoadingPresenter : IDisposable
    {
        void ShowOverlayLoading();
        void HideOverlayLoading();
    }
}