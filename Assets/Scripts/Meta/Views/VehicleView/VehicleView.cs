using System;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.ViewConfigs;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class VehicleView : MonoBehaviour, IVehicleView
{
    [SerializeField] private VehiclesViewConfig vehiclesViewConfig;
    [SerializeField] private WheelsViewConfig wheelsViewConfig;
    [SerializeField] Vector3 commonPosition;
    [SerializeField] Vector3 wheelsChangingPosition;
    public Transform cameraTarget;
    
    private VehicleViewBody _vehicleViewBody;
    
    private AsyncOperationHandle<GameObject> _vehicleViewBodyHandle;
    private AsyncOperationHandle<GameObject> _leftWheelHandle;
    private AsyncOperationHandle<GameObject> _rightWheelHandle;

    public async UniTask ChangeWheels(WheelsDataView wheelsDataView)
    {
        var viewData = wheelsViewConfig.wheels.Find(x => x.wheelsId == wheelsDataView.Id);
        
        if (!_leftWheelHandle.IsDone)
            await _leftWheelHandle;
        
        if (!_rightWheelHandle.IsDone)
            await _rightWheelHandle;
        
        if(_leftWheelHandle.IsValid())
            _leftWheelHandle.Release();
        
        if(_rightWheelHandle.IsValid())
            _rightWheelHandle.Release();

        _leftWheelHandle = viewData.left.LoadAssetAsync<GameObject>();
        _rightWheelHandle = viewData.right.LoadAssetAsync<GameObject>();

        await _leftWheelHandle;
        await _rightWheelHandle;
        
        _vehicleViewBody.SetWheels(_leftWheelHandle.Result, _rightWheelHandle.Result);
    }

    public async UniTask ChangeVehicle(VehicleDataView vehicleDataView)
    {
        var vehicleViewConfig = vehiclesViewConfig.vehicles.Find(x => x.id == vehicleDataView.Id);
        if(_vehicleViewBody)
            Destroy(_vehicleViewBody.gameObject);

        if (!_vehicleViewBodyHandle.IsDone)
            await _vehicleViewBodyHandle;
        
        if(_vehicleViewBodyHandle.IsValid())
            _vehicleViewBodyHandle.Release();

        _vehicleViewBodyHandle = vehicleViewConfig.assetReference.LoadAssetAsync<GameObject>();
        await _vehicleViewBodyHandle;

        _vehicleViewBody = Instantiate(_vehicleViewBodyHandle.Result, this.transform, false).GetComponent<VehicleViewBody>();
        
        _vehicleViewBody.gameObject.transform.localPosition = vehicleViewConfig.defaultPosition;
        _vehicleViewBody.gameObject.SetActive(false);
    }

    public void SetCommonPosition() => cameraTarget.localPosition = commonPosition;
    public void SetWheelsChangingPosition() => cameraTarget.localPosition = wheelsChangingPosition;

    public void ShowVehicle() => _vehicleViewBody?.gameObject.SetActive(true);
    public void HideVehicle() => _vehicleViewBody?.gameObject.SetActive(false);

    public void OnDestroy()
    {
        if(_leftWheelHandle.IsValid())
            _leftWheelHandle.Release();
        
        if(_rightWheelHandle.IsValid())
            _rightWheelHandle.Release();
        
        if(_vehicleViewBodyHandle.IsValid())
            _vehicleViewBodyHandle.Release();
    }
}
