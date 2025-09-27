using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "WheelsPrefabConfig", menuName = "Scriptable Objects/WheelsPrefabConfig")]
    public class WheelsPrefabConfig : ScriptableObject
    {
        public List<Data> wheelsConfig;

        [Serializable]
        public class Data
        {
            public string vehicleId;
            public string wheelsId;
            public GameObject left;
            public GameObject right;
        }
    }
}
