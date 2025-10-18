using System;

namespace Meta.DataConfigs
{
    public class LocalProfile
    {
        public long hard;
        public long soft;

        public string currentVehicleId;
        public ProfileDataConfig.VehicleData[] vehiclesData;
        public ProfileDataConfig.WheelsData[] wheelsData;

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