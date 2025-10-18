using System;

namespace Meta.UseCases
{
    public interface IUseCase
    {
        event Action OnShowPresenter;
        event Action OnHidePresenter;
        void ShowPresenter();
        void HidePresenter();
    }
}