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
    private WheelsChangingView _wheelsChangingView;

    public void Set(WheelsDataView wheelsDataView, WheelsChangingView wheelsChangingView)
    {
        _wheelsDataView = wheelsDataView;
        titleText.text = _wheelsDataView.Id;
        statusText.text = _wheelsDataView.Status;
        priceText.text = _wheelsDataView.Price.ToString();

        _wheelsChangingView = wheelsChangingView;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _wheelsChangingView.TryWheels(_wheelsDataView);
    }
}
