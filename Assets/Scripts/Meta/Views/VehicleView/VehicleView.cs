using Meta.Presenters;
using Meta.ViewConfigs;
using UnityEngine;

public class VehicleView : MonoBehaviour, IVehicleView
{
    [SerializeField] private VehiclesViewConfig vehiclesViewConfig;
    [SerializeField] private WheelsViewConfig wheelsViewConfig;
    [SerializeField] Vector3 commonPosition;
    [SerializeField] Vector3 wheelsChangingPosition;
    public Transform cameraTarget;
    
    private VehiclePresenter _vehiclePresenter;
    private VehicleViewBody _vehicleViewBody;

    public void ChangeWheels(WheelsDataView wheelsDataView)
    {
        var viewData = wheelsViewConfig.wheels.Find(x => x.wheelsId == wheelsDataView.Id);
        _vehicleViewBody.SetWheels(viewData.left, viewData.right);
    }

    public void ChangeVehicle(VehicleDataView vehicleDataView)
    {
        var vehicleViewConfig = vehiclesViewConfig.vehicles.Find(x => x.id == vehicleDataView.Id);
        if(_vehicleViewBody)
            Destroy(_vehicleViewBody.gameObject);
        _vehicleViewBody = Instantiate(vehicleViewConfig.prefab, this.transform, false).GetComponent<VehicleViewBody>();
        _vehicleViewBody.gameObject.transform.localPosition = vehicleViewConfig.defaultPosition;
    }

    public void SetCommonPosition() => cameraTarget.localPosition = commonPosition;
    public void SetWheelsChangingPosition() => cameraTarget.localPosition = wheelsChangingPosition;
}
