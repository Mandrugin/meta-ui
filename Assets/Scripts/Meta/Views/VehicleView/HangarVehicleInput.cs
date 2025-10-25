using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class HangarVehicleInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CinemachineInputAxisController inputAxisController;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        inputAxisController.enabled = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputAxisController.enabled = false;
    }
}
