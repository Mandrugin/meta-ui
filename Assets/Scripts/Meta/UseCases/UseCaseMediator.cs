using System;
using Meta.Entities;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    internal class UseCaseMediator
    {
        public event Action<Vehicle> OnCurrentVehicleChanged = delegate { };
        public void ChangeCurrentVehicle(Vehicle vehicle)
        {
            OnCurrentVehicleChanged(vehicle);
        }

        public event Action<Wheels> OnCurrentWheelsChanged = delegate { };
        public void ChangeCurrentWheels(Wheels setWheels)
        {
            OnCurrentWheelsChanged(setWheels);
        }
        
        public event Action OnShowWheelsChanging = delegate { };
        public void ShowWheelsChanging()
        {
            OnShowWheelsChanging();
        }

        public event Action OnHideWheelsChanging = delegate { };
        public void HideWheelsChanging()
        {
            OnHideWheelsChanging();
        }
    }
}
