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
    private CancellationTokenSource _cancellationTokenSource;

    [Inject]
    private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
    {
        elementPrefab.gameObject.SetActive(false);
        _wheelsChangingPresenter = wheelsChangingPresenter;
        _wheelsChangingPresenter.OnStartUseCase += Show;
        _wheelsChangingPresenter.OnFinishUseCase += Hide;
        _wheelsChangingPresenter.OnSetAvailable += OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable += OnOnBuyAvailable;
        
        _cancellationTokenSource = new CancellationTokenSource();

        setButton.onClick.AddListener(() => _wheelsChangingPresenter.SetWheels().Forget());
        //buyButton.onClick.AddListener(() => wheelsChangingPresenter.BuyWheel());
    }

    private void OnDestroy()
    {
        _wheelsChangingPresenter.OnStartUseCase -= Show;
        _wheelsChangingPresenter.OnFinishUseCase -= Hide;
        _wheelsChangingPresenter.OnSetAvailable -= OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable -= OnOnBuyAvailable;
    }

    private void OnOnSetAvailable(bool isAvailable)
    {
        setButton.gameObject.SetActive(isAvailable);
    }

    private void OnOnBuyAvailable(bool isAvailable)
    {
        buyButton.gameObject.SetActive(isAvailable);
    }

    private void OnWheelsListChanged(List<WheelsDataView> wheelsDataViews)
    {
        // TODO: instead of destroying an old list of GOs and creating a new one we can reuse old GOs
        foreach (var element in _elements)
        {
            Destroy(element.gameObject);
        }
        
        _elements.Clear();

        foreach (var wheelsDataView in wheelsDataViews)
        {
            var element =  Instantiate(elementPrefab, elementPrefab.transform.parent);
            element.Set(wheelsDataView, this);
            element.gameObject.SetActive(true);
            _elements.Add(element);
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        ShowWheels().Forget();
    }

    private void Hide()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        gameObject.SetActive(false);
    }

    private async UniTaskVoid ShowWheels()
    {
        var wheelsDataView = await _wheelsChangingPresenter.GetWheelsDataView(_cancellationTokenSource.Token);
        OnWheelsListChanged(wheelsDataView);
    }

    public async UniTask<bool> TryWheels(WheelsDataView wheelsDataView)
    {
        return await _wheelsChangingPresenter.TryOutWheels(wheelsDataView);
    }
}