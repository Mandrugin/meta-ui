using System.Collections.Generic;
using Meta.Configs;
using Meta.Presenters;
using UnityEditor;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class VehicleView : MonoBehaviour
{
    [Inject] private VehiclesPrefabConfig _vehiclesPrefabConfig;
    [Inject] private WheelsPrefabConfig _wheelsPrefabConfig;
    [Inject] private VehiclePresenter _vehiclePresenter;
    
    [SerializeField] private List<GameObject> leftWheels;
    [SerializeField] private List<GameObject> rightWheels;
    
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
        
        
        switch (wheelsDataView.Id)
        {
            case "small":
                SetWheels(0);
                break;
            case "medium":
                SetWheels(1);
                break;
            case "large":
                SetWheels(2);
                break;
        }
    }

    private void SetWheels(int index)
    {
        DeleteWheels();

        var fl = Instantiate(leftWheels[index], _wheelSlotFL);
        var fr = Instantiate(rightWheels[index], _wheelSlotFR);
        var rl = Instantiate(leftWheels[index], _wheelSlotRL);
        var rr = Instantiate(rightWheels[index], _wheelSlotRR);
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

    [MenuItem("Custom/Rnadom Wheels")]
    private static void SetRandomWheels()
    {
        FindAnyObjectByType<VehicleView>().SetWheels(Random.Range(0, 5));
    }

    [MenuItem("Custom/Clean")]
    private static void Clean()
    {
        FindAnyObjectByType<VehicleView>().DeleteWheels();
    }
}
