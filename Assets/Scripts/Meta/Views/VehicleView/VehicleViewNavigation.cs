using System;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

public class VehicleNavigationView : MonoBehaviour, IVehicleNavigationView
{
    [SerializeField] private Button nextVehicleButton;
    [SerializeField] private Button prevVehicleButton;

    public event Action OnNextVehicle = delegate { };
    public event Action OnPrevVehicle = delegate { };

    public void Awake()
    {
        nextVehicleButton.onClick.AddListener(() => OnNextVehicle.Invoke());
        prevVehicleButton.onClick.AddListener(() => OnPrevVehicle.Invoke());            
    }
}
