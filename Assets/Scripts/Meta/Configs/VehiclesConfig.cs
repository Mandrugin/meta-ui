using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "VehiclesConfig", menuName = "Scriptable Objects/VehiclesConfig")]
    public class VehiclesConfig : ScriptableObject
    {
        public List<VehiclesConfigData> vehicles;

        [Serializable]
        public class VehiclesConfigData
        {
            public string id;
            public GameObject prefab;
        }
    }
}