using System.Collections.Generic;
using Meta.Presenters;
using UnityEngine;
using VContainer;

namespace Meta.Views
{
    public class WheelsChangingView : MonoBehaviour
    {
        [SerializeField] private WheelsChangingViewElement elementPrefab;
        [SerializeField] private List<WheelsChangingViewElement> elements =  new();

        private WheelsChangingPresenter _wheelsChangingPresenter;

        [Inject]
        private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
        {
            elementPrefab.gameObject.SetActive(false);
            _wheelsChangingPresenter = wheelsChangingPresenter;
            _wheelsChangingPresenter.WheelsListChanged += OnWheelsListChanged;
            _wheelsChangingPresenter.UpdateWheelsData().Forget();
        }

        private void OnDestroy()
        {
            _wheelsChangingPresenter.WheelsListChanged -= OnWheelsListChanged;
        }

        private void OnWheelsListChanged(List<WheelsDataView> wheelsDataViews)
        {
            // TODO: instead of destroying an old list of GOs and creating a new one we can reuse old GOs
            foreach (var element in elements)
            {
                Destroy(element.gameObject);
            }
            
            elements.Clear();

            foreach (var wheelsDataView in wheelsDataViews)
            {
                var element =  Instantiate(elementPrefab, transform);
                element.wheelsIdText.text = wheelsDataView.Id;
                element.gameObject.SetActive(true);
            }
        }
    }
}
