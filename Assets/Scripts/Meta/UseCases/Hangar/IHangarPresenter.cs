using System;

namespace Meta.UseCases
{
    public interface IHangarPresenter
    {
        event Action OnShowWheelsChanging;
        event Action OnHideWheelsChanging;
        void ChangeHard(long hard);
        void ChangeSoft(long soft);
    }
}