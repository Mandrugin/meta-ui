using System;

namespace UseCases
{
    public interface IWheelsChangingUseCase
    {
        event Action<WheelsData[]> WheelsDataUpdated;
        event Action<WheelsData> WheelsTriedOut;
        event Action<WheelsData> WheelsSet;
        void UpdateWheelsData();
        void TryWheelsOut(int wheelIndex);
        void SetWheels(int wheelIndex);
    }
}