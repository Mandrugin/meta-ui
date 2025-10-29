using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialOffersDataConfig", menuName = "Scriptable Objects/SpecialOffersDataConfig")]
public class SpecialOffersDataConfig : ScriptableObject
{
    public List<Data> specialOffers;
    
    [Serializable]
    public class Data
    {
        public string id;
    }
}
