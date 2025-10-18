using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class WheelsChangingView : MonoBehaviour
{
    [SerializeField] private WheelsChangingViewElement elementPrefab;
    [SerializeField] private Button setButton;
    [SerializeField] private Button buyButton;
    
    private readonly List<WheelsChangingViewElement> _elements =  new();

    private WheelsChangingPresenter _wheelsChangingPresenter;

    [Inject]
    private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
    {
        elementPrefab.gameObject.SetActive(false);
        _wheelsChangingPresenter = wheelsChangingPresenter;
        _wheelsChangingPresenter.OnShowPresenter += Show;
        _wheelsChangingPresenter.OnHidePresenter += Hide;
        _wheelsChangingPresenter.OnSetAvailable += OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable += OnOnBuyAvailable;
        _wheelsChangingPresenter.OnWheelsListChanged += ChangeWheelsList;
        _wheelsChangingPresenter.OnSetActiveWheels += SetActiveWheels;

        setButton.onClick.AddListener(() => _wheelsChangingPresenter.SetWheels().Forget());
        buyButton.onClick.AddListener(() => _wheelsChangingPresenter.BuyWheels().Forget());
    }

    private void OnDestroy()
    {
        _wheelsChangingPresenter.OnShowPresenter -= Show;
        _wheelsChangingPresenter.OnHidePresenter -= Hide;
        _wheelsChangingPresenter.OnSetAvailable -= OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable -= OnOnBuyAvailable;
        _wheelsChangingPresenter.OnWheelsListChanged -= ChangeWheelsList;
        _wheelsChangingPresenter.OnSetActiveWheels -= SetActiveWheels;
    }

    private void OnOnSetAvailable(bool isAvailable)
    {
        setButton.gameObject.SetActive(isAvailable);
    }

    private void OnOnBuyAvailable(bool isAvailable)
    {
        buyButton.gameObject.SetActive(isAvailable);
    }

    private void ChangeWheelsList(List<WheelsDataView> wheelsDataViews)
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

    public void SetActiveWheels(WheelsDataView wheelsDataView)
    {
        foreach (var element in _elements)
        {
            var isActive = element.GetWheelsDataView().Id == wheelsDataView.Id;
            element.SetActive(isActive);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        ShowWheels();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowWheels()
    {
        _wheelsChangingPresenter.UpdateWheelsData(destroyCancellationToken).Forget();
    }

    public async UniTask<bool> TryWheels(WheelsDataView wheelsDataView, CancellationToken cancellationToken)
    {
        return await _wheelsChangingPresenter.TryOutWheels(wheelsDataView, cancellationToken);
    }
}