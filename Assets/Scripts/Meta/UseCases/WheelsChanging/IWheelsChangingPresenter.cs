using System;
using System.Collections.Generic;

namespace Meta.UseCases
{
    public interface IWheelsChangingPresenter
    {
        event Action OnSetWheels;
        event Action OnBuyWheels;
        event Action<WheelsData> OnWheelsTriedOut;
        void SetSetAvailable(bool isAvailable);
        void SetBuyAvailable(bool isAvailable);
        void ChangeWheelsList(List<WheelsData> allWheelsData, List<WheelsData> boughtWheelsData, WheelsData setWheelsData);
    }
}