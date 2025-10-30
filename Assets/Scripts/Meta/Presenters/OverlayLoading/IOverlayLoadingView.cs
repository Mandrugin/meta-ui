using System;

namespace Meta.Presenters
{
    public interface IOverlayLoadingView: IDisposable
    {
        void ShowOverlayLoading();
        void HideOverlayLoading();
    }
}