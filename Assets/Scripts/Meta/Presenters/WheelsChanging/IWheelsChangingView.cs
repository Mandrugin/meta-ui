using System;
using System.Collections.Generic;
using Meta.Presenters;

public interface IWheelsChangingView
{
    event Action<WheelsDataView> OnWheelsTriedOut;
    event Action OnSetWheels;
    event Action OnBuyWheels;
    void SetCurrentWheels(WheelsDataView wheelsDataView);
    void ChangeWheelsList(List<WheelsDataView> wheelsDataViews);
    void SetSetAvailable(bool isAvailable);
    void SetBuyAvailable(bool isAvailable);
}