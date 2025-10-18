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
        _wheelsChangingPresenter.OnStartUseCase += Show;
        _wheelsChangingPresenter.OnFinishUseCase += Hide;
        _wheelsChangingPresenter.OnSetAvailable += OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable += OnOnBuyAvailable;
        _wheelsChangingPresenter.OnWheelsListChanged += ChangeWheelsList;
        
        setButton.onClick.AddListener(() =>
        {
            _wheelsChangingPresenter.SetWheels().ContinueWith(result => Debug.Log(result ? "success" : "failure"));
        });
        //buyButton.onClick.AddListener(() => wheelsChangingPresenter.BuyWheel());
    }

    private void OnDestroy()
    {
        _wheelsChangingPresenter.OnStartUseCase -= Show;
        _wheelsChangingPresenter.OnFinishUseCase -= Hide;
        _wheelsChangingPresenter.OnSetAvailable -= OnOnSetAvailable;
        _wheelsChangingPresenter.OnBuyAvailable -= OnOnBuyAvailable;
        _wheelsChangingPresenter.OnWheelsListChanged -= ChangeWheelsList;
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
        gameObject.SetActive(false);
    }

    private async UniTaskVoid ShowWheels()
    {
        var wheelsDataView = await _wheelsChangingPresenter.GetWheelsDataView(destroyCancellationToken);
        ChangeWheelsList(wheelsDataView);
    }

    public async UniTask<bool> TryWheels(WheelsDataView wheelsDataView, CancellationToken cancellationToken)
    {
        return await _wheelsChangingPresenter.TryOutWheels(wheelsDataView, cancellationToken);
    }
}