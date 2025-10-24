using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WheelsChangingViewElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject priceIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image imageBg;
    [SerializeField] private Sprite commonBg;
    [SerializeField] private Sprite currentBg;
    [SerializeField] private Sprite setBg;

    private WheelsDataView _wheelsDataView;
    private WheelsChangingView _wheelsChangingView;
    
    public void SetWheelsDataView(WheelsDataView wheelsDataView, WheelsChangingView wheelsChangingView)
    {
        _wheelsDataView = wheelsDataView;
        titleText.text = _wheelsDataView.Id;
        
        if(wheelsDataView.IsBought == false)
        {
            priceIcon.SetActive(true);
            priceText.gameObject.SetActive(true);
            priceText.text = _wheelsDataView.Price.ToString();
        }
        else
        {
            priceIcon.SetActive(false);
            priceText.gameObject.SetActive(false);
        }
        
        imageBg.sprite = _wheelsDataView.IsSet ? setBg : commonBg;

        _wheelsChangingView = wheelsChangingView;
    }

    public WheelsDataView GetWheelsDataView()
    {
        return _wheelsDataView;
    }

    public void SetActive(bool active)
    {
        var bg = _wheelsDataView.IsSet ? setBg : commonBg;
        imageBg.sprite = active ? currentBg : bg;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _wheelsChangingView.TryWheelsOut(_wheelsDataView);
    }
}
