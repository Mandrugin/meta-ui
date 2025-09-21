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
            _wheelsChangingPresenter.OnStartUseCase += Show;
            _wheelsChangingPresenter.OnFinishUseCase += Hide;
            _wheelsChangingPresenter.WheelsListChanged += OnWheelsListChanged;
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
                element.Set(wheelsDataView, _wheelsChangingPresenter);
                element.gameObject.SetActive(true);
                elements.Add(element);
            }
        }

        private void Show()
        {
            _wheelsChangingPresenter.UpdateWheelsData().Forget();
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
