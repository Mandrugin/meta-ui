using Cysharp.Threading.Tasks;
using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WheelsChangingViewElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject activeFlag;

    private WheelsDataView _wheelsDataView;
    private WheelsChangingView _wheelsChangingView;
    
    public void SetWheelsDataView(WheelsDataView wheelsDataView, WheelsChangingView wheelsChangingView)
    {
        _wheelsDataView = wheelsDataView;
        titleText.text = _wheelsDataView.Id;
        statusText.text = _wheelsDataView.Status;
        priceText.text = _wheelsDataView.Price.ToString();

        _wheelsChangingView = wheelsChangingView;
    }

    public WheelsDataView GetWheelsDataView()
    {
        return _wheelsDataView;
    }

    public void SetActive(bool active)
    {
        activeFlag.SetActive(active);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _wheelsChangingView.TryWheels(_wheelsDataView, destroyCancellationToken).Forget();
        _wheelsChangingView.SetActiveWheels(_wheelsDataView);
    }
}
