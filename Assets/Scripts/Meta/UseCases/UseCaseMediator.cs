using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    public class UseCaseMediator
    {
        public event Action OnShowWheelsChanging = delegate { };
        public event Action OnHideWheelsChanging = delegate { };
        public void ShowWheelsChanging() => OnShowWheelsChanging.Invoke();
        public void HideWheelsChanging() => OnHideWheelsChanging.Invoke();

        public event Action<VehicleData> OnCurrentVehicleChanged = delegate { };
        public void ChangeCurrentVehicle(VehicleData vehicleData)
        {
            Debug.Log($"Change current vehicle {vehicleData.Id}");
            OnCurrentVehicleChanged(vehicleData);
        }

        public event Action<WheelsData> OnCurrentWheelsChanged = delegate { };
        public void ChangeCurrentWheels(WheelsData wheelsData) => OnCurrentWheelsChanged(wheelsData);
        
        public event Action<List<WheelsData>, List<WheelsData>, WheelsData> OnWheelsListChanged = delegate { };
        public void ChangeWheelsList(List<WheelsData> allWheels, List<WheelsData> boughtWheels, WheelsData setWheels)
            => OnWheelsListChanged(allWheels, boughtWheels, setWheels);
    }
}