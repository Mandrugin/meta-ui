using Meta.Presenters;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HangarView : MonoBehaviour
{
    [Inject] private HangarPresenter _hangarPresenter;
    
    [SerializeField] private Button startWheelsChanging;
    [SerializeField] private Button finishWheelsChanging;

    private void Awake()
    {
        startWheelsChanging.onClick.AddListener(() => _hangarPresenter.StartWheelsChanging());
        finishWheelsChanging.onClick.AddListener(() => _hangarPresenter.FinishWheelsChanging());
    }
}
