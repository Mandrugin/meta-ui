using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "VehiclesPrefabConfig", menuName = "Scriptable Objects/VehiclesPrefabConfig")]
    public class VehiclesPrefabConfig : ScriptableObject
    {
        public List<Data> vehicles;

        [Serializable]
        public class Data
        {
            public string id;
            public GameObject prefab;
        }
    }
}
