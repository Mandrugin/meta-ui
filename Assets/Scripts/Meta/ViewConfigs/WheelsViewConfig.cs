using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Meta.ViewConfigs
{
    [CreateAssetMenu(fileName = "WheelsViewConfig", menuName = "Scriptable Objects/WheelsViewConfig")]
    public class WheelsViewConfig : ScriptableObject
    {
        [FormerlySerializedAs("wheelsConfig")] public List<Data> wheels;

        [Serializable]
        public class Data
        {
            public string vehicleId;
            public string wheelsId;
            public Vector3 position;
            public AssetReferenceGameObject left;
            public AssetReferenceGameObject right;
        }
    }
}
