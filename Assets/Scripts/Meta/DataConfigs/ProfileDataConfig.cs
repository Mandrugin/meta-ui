using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.DataConfigs
{
    [CreateAssetMenu(fileName = "ProfileDataConfig", menuName = "Scriptable Objects/ProfileDataConfig")]
    public class ProfileDataConfig : ScriptableObject
    {
        public long hard;
        public long soft;

        public string setVehicleId;
        public List<VehicleData> vehiclesData;
        public List<WheelsData> wheelsData;

        [Serializable]
        public class VehicleData
        {
            public string id;
            public string currentWheelsId;
        }
        
        [Serializable]
        public class WheelsData
        {
            public string id;
        }
    }
}
