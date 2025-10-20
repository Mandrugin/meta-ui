using System;
using Meta.Entities;
using UnityEngine;
using UnityEngine.Scripting;

namespace Meta.UseCases
{
    [Preserve]
    internal class UseCaseMediator
    {
        public event Action<Vehicle> OnCurrentVehicleChanged = delegate { };
        public void ChangeCurrentVehicle(Vehicle vehicle)
        {
            Debug.Log($"Change current vehicle {vehicle.Id}");
            OnCurrentVehicleChanged(vehicle);
        }

        public event Action<Wheels> OnCurrentWheelsChanged = delegate { };
        public void ChangeCurrentWheels(Wheels setWheels)
        {
            OnCurrentWheelsChanged(setWheels);
        }
    }
}