using System;
using UnityEngine;

namespace Meta.DataConfigs
{
    [CreateAssetMenu(fileName = "ProfileDataConfig", menuName = "Scriptable Objects/ProfileDataConfig")]
    public class ProfileDataConfig : ScriptableObject
    {
        public long hard;
        public long soft;

        public string currentVehicleId;
        public VehicleData[] vehiclesData;
        public WheelsData[] wheelsData;

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

    public static class ProfileDataConfigsExtensions
    {
        public static LocalProfile ToLocalProfile(this ProfileDataConfig config)
        {
            return new LocalProfile
            {
                hard = config.hard,
                soft = config.soft,
                currentVehicleId = config.currentVehicleId,
                vehiclesData = config.vehiclesData,
                wheelsData = config.wheelsData
            };
        }
    }
}
