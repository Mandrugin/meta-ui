using System;
using System.Collections.Generic;
using UnityEngine;

namespace Meta.Configs
{
    [CreateAssetMenu(fileName = "ViewWheelsConfig", menuName = "Scriptable Objects/ViewWheelsConfig")]
    public class ViewWheelsConfig : ScriptableObject
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
