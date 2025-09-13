using System;

namespace UseCases
{
    public class WheelsChangingUseCase : IWheelsChangingUseCase
    {
        private WheelsData[] _wheelsData;
        public event Action<WheelsData[]> WheelsDataUpdated;
        public event Action<WheelsData> WheelsTriedOut;
        public event Action<WheelsData> WheelsSet;

        public void UpdateWheelsData()
        {
            
        }
        
        public void TryWheelsOut(int wheelIndex)
        {
            
        }

        public void SetWheels(int wheelIndex)
        {
            
        }

        protected virtual void OnWheelsDataUpdated(WheelsData[] obj)
        {
            WheelsDataUpdated?.Invoke(obj);
        }

        protected virtual void OnWheelsTriedOut(WheelsData obj)
        {
            WheelsTriedOut?.Invoke(obj);
        }

        protected virtual void OnWheelsSet(WheelsData obj)
        {
            WheelsSet?.Invoke(obj);
        }
    }
}
