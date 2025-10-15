using UnityEngine;

namespace Meta.DataConfigs
{
    [CreateAssetMenu(fileName = "ProfileDataConfig", menuName = "Scriptable Objects/ProfileDataConfig")]
    public class ProfileDataConfig : ScriptableObject
    {
        public long hard;
        public long soft;

        public string[] vehicleIds;
        public string[] wheelsIds;
    }
}
