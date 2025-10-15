using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelsChangingViewElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI priceText;
    
    private WheelsDataView _wheelsDataView;
    private WheelsChangingPresenter _wheelsChangingPresenter;

    public void Set(WheelsDataView wheelsDataView, WheelsChangingPresenter wheelsChangingPresenter)
    {
        _wheelsDataView = wheelsDataView;
        titleText.text = _wheelsDataView.Id;
        statusText.text = _wheelsDataView.Status;
        priceText.text = _wheelsDataView.Price.ToString();

        _wheelsChangingPresenter = wheelsChangingPresenter;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _wheelsChangingPresenter.TryWheels(_wheelsDataView);
    }
}
