using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SpecialOffersPopUpsConfig", menuName = "Scriptable Objects/SpecialOffersPopUpsConfig")]
public class SpecialOffersPopUpsConfig : ScriptableObject
{
    public List<Data> specialOfferConfigs;
    
    [Serializable]
    public class Data
    {
        public string specialOfferId;
        public AssetReferenceGameObject specialOfferAssetReference;
        public AssetReferenceGameObject congratulationAssetReference;
    }
}
