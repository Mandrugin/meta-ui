using UnityEngine;

public class VehicleViewBody: MonoBehaviour
{
    [SerializeField] private Transform _wheelSlotFL;
    [SerializeField] private Transform _wheelSlotFR;
    [SerializeField] private Transform _wheelSlotRL;
    [SerializeField] private Transform _wheelSlotRR;

    public void SetWheels(GameObject leftWheels, GameObject rightWheels)
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