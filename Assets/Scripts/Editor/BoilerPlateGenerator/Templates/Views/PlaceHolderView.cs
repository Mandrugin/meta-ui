using Meta.Presenters;
using UnityEngine;

namespace Meta.Views
{
    public class PlaceHolderView: MonoBehaviour, IPlaceHolderView
    {
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
