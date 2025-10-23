using System;
using System.Linq;
using Meta.Presenters;
using Meta.ViewConfigs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleNavigationView : MonoBehaviour, IVehicleNavigationView
{
    [SerializeField] private Button nextVehicleButton;
    [SerializeField] private Button prevVehicleButton;
    [SerializeField] private TextMeshProUGUI vehicleNameText;
    [SerializeField] private VehiclesViewConfig vehiclesViewConfig;

    public event Action OnNextVehicle = delegate { };
    public event Action OnPrevVehicle = delegate { };

    public void SetVehicleName(VehicleDataView vehicleDataView)
        => vehicleNameText.text = vehiclesViewConfig.vehicles.First(x => x.id == vehicleDataView.Id).id;

    public void Awake()
    {
        nextVehicleButton.onClick.AddListener(() => OnNextVehicle.Invoke());
        prevVehicleButton.onClick.AddListener(() => OnPrevVehicle.Invoke());            
    }
}
