using System;
using UnityEngine;

namespace Meta.DataConfigs
{
    [CreateAssetMenu(fileName = "VehiclesDataConfig", menuName = "Scriptable Objects/VehiclesDataConfig")]
    public class VehiclesDataConfig : ScriptableObject
    {
        public Data[] vehicles;
    
        [Serializable]
        public class Data
        {
            public string id;
            public string displayName;
            public long price;
        }
    }
}
