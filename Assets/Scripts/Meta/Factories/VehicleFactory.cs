using Meta.UseCases;
using UnityEngine;

namespace Meta.Factories
{
    public class VehicleFactory : MonoBehaviour, IVehicleFactory
    {
        [SerializeField] private VehicleView vehicleView;
        [SerializeField] private VehicleViewNavigation vehicleViewNavigation;
    }
}
