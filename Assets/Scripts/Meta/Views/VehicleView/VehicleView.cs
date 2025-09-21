using System.Collections.Generic;
using UnityEngine;

public class VehicleView : MonoBehaviour
{
    [SerializeField] private List<GameObject> leftWheels;
    [SerializeField] private List<GameObject> rightWheels;
    
    public Transform _wheelSlotFL;
    public Transform _wheelSlotFR;
    public Transform _wheelSlotRL;
    public Transform _wheelSlotRR;
    
    
}
