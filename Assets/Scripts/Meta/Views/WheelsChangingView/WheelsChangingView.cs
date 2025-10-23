using System;
using System.Collections.Generic;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;

public class WheelsChangingView : MonoBehaviour, IWheelsChangingView
{
    [SerializeField] private WheelsChangingViewElement elementPrefab;
    [SerializeField] private Button setButton;
    [SerializeField] private Button buyButton;
    
    private readonly List<WheelsChangingViewElement> _elements =  new();

    public event Action<WheelsDataView> OnWheelsTriedOut = delegate { };
    public event Action OnSetWheels = delegate { };
    public event Action OnBuyWheels = delegate { };


    private void Awake()
    {
        elementPrefab.gameObject.SetActive(false);
        setButton.onClick.AddListener(OnSetWheelsInvocator);
        buyButton.onClick.AddListener(OnBuyWheelsInvocator);
    }

    private void OnSetWheelsInvocator() => OnSetWheels.Invoke();
    private void OnBuyWheelsInvocator() => OnBuyWheels.Invoke();

    public void TryWheelsOut(WheelsDataView wheelsDataView)
    {
        SetCurrentWheels(wheelsDataView);
        OnWheelsTriedOut.Invoke(wheelsDataView);
    }

    public void SetCurrentWheels(WheelsDataView wheelsDataView)
    {
        foreach (var element in _elements)
        {
            var isActive = element.GetWheelsDataView().Id == wheelsDataView.Id;
            element.SetActive(isActive);
        }        
    }

    public void ChangeWheelsList(List<WheelsDataView> wheelsDataViews)
    {
        // TODO: instead of destroying an old list of GOs and creating a new one we can reuse old GOs
        foreach (var element in _elements)
        {
            Destroy(element.gameObject);
        }
        
        _elements.Clear();

        foreach (var wheelsDataView in wheelsDataViews)
        {
            var element = Instantiate(elementPrefab, elementPrefab.transform.parent);
            element.SetWheelsDataView(wheelsDataView, this);
            element.gameObject.SetActive(true);
            _elements.Add(element);
        }
    }

    public void SetSetAvailable(bool isAvailable)
    {
        setButton.gameObject.SetActive(isAvailable);
    }

    public void SetBuyAvailable(bool isAvailable)
    {
        buyButton.gameObject.SetActive(isAvailable);
    }
}