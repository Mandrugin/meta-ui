using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using Meta.ViewConfigs;
using UnityEngine;


namespace Meta.Views
{
    public class SpecialOffersView: MonoBehaviour, ISpecialOffersView
    {
        [SerializeField] private SpecialOffersViewConfig specialOffersViewConfig;
        [SerializeField] private SpecialOffersElementView specialOffersElementPrefab;
        [SerializeField] private Transform parent;

        private readonly List<SpecialOffersElementView> specialOffersElements = new();
        
        public event Action<string> OnClickSpecialOffer = delegate { };
        
        private void OnClickSpecialOfferInvocator(string specialOffer) => OnClickSpecialOffer.Invoke(specialOffer);

        public void AddSpecialOffer(SpecialOfferDataView specialOfferId)
        {
            var specialOfferElementView = Instantiate(specialOffersElementPrefab, parent);
            specialOfferElementView.Init(specialOfferId.Id,
                specialOffersViewConfig.specialOffers.First(x => x.id == specialOfferId.Id).prefab).Forget();
            specialOfferElementView.onClick += OnClickSpecialOfferInvocator;
            specialOffersElements.Add(specialOfferElementView);
        }

        public void RemoveSpecialOffer(SpecialOfferDataView specialOfferId)
        {
            var specialOffersElementView = specialOffersElements.First(x => x.Id == specialOfferId.Id);
            specialOffersElementView.onClick -= OnClickSpecialOfferInvocator;
            specialOffersElements.Remove(specialOffersElementView);
            Destroy(specialOffersElementView.gameObject);
        }

        public void AddSpecialOffers(List<SpecialOfferDataView> specialOfferIds)
        {
            foreach (var specialOfferId in specialOfferIds)
            {
                AddSpecialOffer(specialOfferId);
            }
        }

        public void RemoveSpecialOffers(List<SpecialOfferDataView> specialOfferIds)
        {
            foreach (var specialOfferId in specialOfferIds)
            {
                RemoveSpecialOffer(specialOfferId);
            }
        }

        public void Dispose()
        {
            if(gameObject)
                Destroy(gameObject);
        }
    }
}
