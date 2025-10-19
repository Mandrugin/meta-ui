using Meta.Presenters;
using Meta.ViewConfigs;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private Button nextVehicleButton;
    [SerializeField] private Button prevVehicleButton;
    
    [Inject] private VehiclesViewConfig _vehiclesViewConfig;
    [Inject] private WheelsViewConfig _wheelsViewConfig;
    [Inject] private VehiclePresenter _vehiclePresenter;
    
    private VehicleViewBody _vehicleViewBody;

    private void Awake()
    {
        _vehiclePresenter.OnVehicleChanged += OnVehicleChanged;
        _vehiclePresenter.OnWheelsChanged += OnWheelsChanged;
        
        nextVehicleButton.onClick.AddListener(_vehiclePresenter.SetNexVehicle);
        prevVehicleButton.onClick.AddListener(_vehiclePresenter.SetPrevVehicle);
    }

    private void OnDestroy()
    {
        _vehiclePresenter.OnVehicleChanged -= OnVehicleChanged;
        _vehiclePresenter.OnWheelsChanged -= OnWheelsChanged;
    }

    private void OnVehicleChanged(VehicleDataView vehicleDataView)
    {
        var vehicleData = _vehiclesViewConfig.vehicles.Find(x => x.id == vehicleDataView.Id);
        if(_vehicleViewBody)
            Destroy(_vehicleViewBody.gameObject);
        _vehicleViewBody = Instantiate(vehicleData.prefab, this.transform, false).GetComponent<VehicleViewBody>();
        _vehicleViewBody.gameObject.transform.localPosition = vehicleData.defaultPosition;
    }

    private void OnWheelsChanged(WheelsDataView wheelsDataView)
    {
        var viewData = _wheelsViewConfig.wheels.Find(x => x.wheelsId == wheelsDataView.Id);
        _vehicleViewBody.SetWheels(viewData.left, viewData.right);
    }
    
    
}
