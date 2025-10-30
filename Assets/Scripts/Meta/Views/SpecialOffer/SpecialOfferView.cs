using System;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Views
{
    public class SpecialOfferView: MonoBehaviour, ISpecialOfferView
    {
        [SerializeField] private Button getSpecialOfferButton;
        [SerializeField] private Button dismissSpecialOfferButton;
        
        public event Action<string> OnGetSpecialOffer = delegate { };
        public event Action DismissSpecialOffer = delegate { };

        private string _specialOfferId;

        public void Init(string specialOfferId)
        {
            _specialOfferId = specialOfferId;
            getSpecialOfferButton.onClick.AddListener(() => {OnGetSpecialOffer.Invoke(_specialOfferId);});
            dismissSpecialOfferButton.onClick.AddListener(() => dismissSpecialOfferButton.onClick.Invoke());
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
