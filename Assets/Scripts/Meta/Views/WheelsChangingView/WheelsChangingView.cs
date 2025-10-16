using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Meta.Presenters;
using UnityEngine;
using VContainer;

public class WheelsChangingView : MonoBehaviour
{
    [SerializeField] private WheelsChangingViewElement elementPrefab;
    [SerializeField] private List<WheelsChangingViewElement> elements =  new();

    private WheelsChangingPresenter _wheelsChangingPresenter;
    private CancellationTokenSource _cancellationTokenSource;

    [Inject]
    private void Construct(WheelsChangingPresenter wheelsChangingPresenter)
    {
        elementPrefab.gameObject.SetActive(false);
        _wheelsChangingPresenter = wheelsChangingPresenter;
        _wheelsChangingPresenter.OnStartUseCase += Show;
        _wheelsChangingPresenter.OnFinishUseCase += Hide;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void OnDestroy()
    {
        _wheelsChangingPresenter.OnStartUseCase -= Show;
        _wheelsChangingPresenter.OnFinishUseCase -= Hide;
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
            element.Set(wheelsDataView, this);
            element.gameObject.SetActive(true);
            elements.Add(element);
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

    public void TryWheels(WheelsDataView wheelsDataView)
    {
        _wheelsChangingPresenter.TryWheels(wheelsDataView);
    }
}