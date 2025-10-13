using Meta.Configs;
using Meta.Presenters;
using UnityEngine;
using VContainer;

public class VehicleView : MonoBehaviour
{
    [Inject] private VehiclesViewConfig _vehiclesViewConfig;
    [Inject] private WheelsViewConfig _wheelsViewConfig;
    [Inject] private VehiclePresenter _vehiclePresenter;
    
    [SerializeField] private Transform _wheelSlotFL;
    [SerializeField] private Transform _wheelSlotFR;
    [SerializeField] private Transform _wheelSlotRL;
    [SerializeField] private Transform _wheelSlotRR;

    private void Awake()
    {
        _vehiclePresenter.OnTriedOutWheels += OnTriedOutWheels;
    }

    private void OnDestroy()
    {
        _vehiclePresenter.OnTriedOutWheels -= OnTriedOutWheels;
    }

    private void OnTriedOutWheels(WheelsDataView wheelsDataView)
    {
        var viewData = _wheelsViewConfig.wheels.Find(x => x.wheelsId == wheelsDataView.Id);
        SetWheels(viewData.left, viewData.right);
    }

    private void SetWheels(GameObject leftWheels, GameObject rightWheels)
    {
        DeleteWheels();

        var fl = Instantiate(leftWheels, _wheelSlotFL);
        var fr = Instantiate(rightWheels, _wheelSlotFR);
        var rl = Instantiate(leftWheels, _wheelSlotRL);
        var rr = Instantiate(rightWheels, _wheelSlotRR);
    }

    private void DeleteWheels()
    {
        foreach (Transform t in _wheelSlotFL)
            Delete(t.gameObject);
        
        foreach (Transform t in _wheelSlotFR)
            Delete(t.gameObject);
        
        foreach (Transform t in _wheelSlotRL)
            Delete(t.gameObject);
        
        foreach (Transform t in _wheelSlotRR)
            Delete(t.gameObject);
        
        void Delete(GameObject g)
        {
#if UNITY_EDITOR
            DestroyImmediate(g);
#else
            Destroy(g);
#endif
        }        
    }
}
