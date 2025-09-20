using System.Collections.Generic;
using Meta.Presenters;
using UnityEngine;

namespace Meta.Views
{
    public class WheelsChangingView : MonoBehaviour
    {
        [SerializeField] private WheelsChangingViewElement elementPrefab;
        [SerializeField] private List<WheelsChangingViewElement> elements =  new();
        
        WheelsChangingPresenter _wheelsChangingPresenter;

        private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
        {
            _wheelsChangingPresenter = wheelsChangingPresenter;
            _wheelsChangingPresenter.WheelsListChanged += OnWheelsListChanged;
            _wheelsChangingPresenter.UpdateWheelsData().Forget();
        }

        private void OnWheelsListChanged(List<WheelsDataView> wheelsDataViews)
        {
            foreach (var element in elements)
            {
                Destroy(element.gameObject);
            }
            
            elements.Clear();
            
            
        }
    }
}
