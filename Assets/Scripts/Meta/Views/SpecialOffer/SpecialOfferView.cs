using Meta.Presenters;
using UnityEngine;

namespace Meta.Views
{
    public class SpecialOfferView: MonoBehaviour, ISpecialOfferView
    {
        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
