using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "ViewWheelsConfig", menuName = "Scriptable Objects/ViewWheelsConfig")]
    public class WheelsViewConfig : ScriptableObject
    {
        [FormerlySerializedAs("wheelsConfig")] public List<Data> wheels;

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
