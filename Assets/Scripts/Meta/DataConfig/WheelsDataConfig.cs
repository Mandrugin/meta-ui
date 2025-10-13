using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WheelsDataConfig", menuName = "Scriptable Objects/WheelsDataConfig")]
public class WheelsDataConfig : ScriptableObject
{
    public Data[] wheels;
    
    [Serializable]
    public class Data
    {
        public string id;
        public string vehicleId;
        public string displayName;
        public long price;
    }
}
