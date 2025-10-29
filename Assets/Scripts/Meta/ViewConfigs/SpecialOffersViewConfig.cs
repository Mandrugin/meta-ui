using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Meta.ViewConfigs
{
    [CreateAssetMenu(fileName = "SpecialOffersViewConfig", menuName = "Scriptable Objects/SpecialOffersViewConfig")]
    public class SpecialOffersViewConfig : ScriptableObject
    {
        public List<Data> specialOffers;
        
        [Serializable]
        public class Data
        {
            public string id;
            public AssetReferenceGameObject prefab;
        }
    }
}