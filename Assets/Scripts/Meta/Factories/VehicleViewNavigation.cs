using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Factories
{
    public class VehicleViewNavigation : MonoBehaviour
    {
        [SerializeField] private Button nextVehicleButton;
        [SerializeField] private Button prevVehicleButton;

        private VehiclePresenter _vehiclePresenter;

        public void Init(VehiclePresenter vehiclePresenter)
        {
            _vehiclePresenter = vehiclePresenter;
            
            nextVehicleButton.onClick.AddListener(_vehiclePresenter.SetNexVehicle);
            prevVehicleButton.onClick.AddListener(_vehiclePresenter.SetPrevVehicle);            
        }
    }
}