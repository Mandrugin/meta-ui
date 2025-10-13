using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "ViewVehiclesConfig", menuName = "Scriptable Objects/ViewVehiclesConfig")]
    public class VehiclesViewConfig : ScriptableObject
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
