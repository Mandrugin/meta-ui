using System;

namespace Meta.Presenters
{
    public interface IHangarView
    {
        event Action OnShowWheelsChanging;
        event Action OnHideWheelsChanging;
        void ChangeHard(long hard);
        void ChangeSoft(long soft);
    }
}