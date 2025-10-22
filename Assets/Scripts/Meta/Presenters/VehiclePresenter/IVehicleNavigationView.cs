using System;

namespace Meta.Presenters
{
    public interface IVehicleNavigationView
    {
        event Action OnNextVehicle;
        event Action OnPrevVehicle;
    }
}