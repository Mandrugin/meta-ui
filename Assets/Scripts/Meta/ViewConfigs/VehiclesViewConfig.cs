using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Meta.ViewConfigs
{
    [CreateAssetMenu(fileName = "VehiclesViewConfig", menuName = "Scriptable Objects/VehiclesViewConfig")]
    public class VehiclesViewConfig : ScriptableObject
    {
        public List<Data> vehicles;

        [Serializable]
        public class Data
        {
            public string id;
            public AssetReferenceGameObject assetReference;
            public Vector3 defaultPosition;
        }
    }
}
