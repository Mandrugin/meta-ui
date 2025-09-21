using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelsChangingViewElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI wheelsIdText;
    private WheelsDataView _wheelsDataView;
    private WheelsChangingPresenter _wheelsChangingPresenter;

    public void Set(WheelsDataView wheelsDataView, WheelsChangingPresenter wheelsChangingPresenter)
    {
        _wheelsDataView = wheelsDataView;
        wheelsIdText.text = _wheelsDataView.Id;

        _wheelsChangingPresenter = wheelsChangingPresenter;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _wheelsChangingPresenter.TryWheels(_wheelsDataView);
    }
}
