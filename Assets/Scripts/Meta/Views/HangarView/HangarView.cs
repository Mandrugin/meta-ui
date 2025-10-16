using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class HangarView : MonoBehaviour
{
    [Inject] private HangarPresenter _hangarPresenter;

    [SerializeField] private TextMeshProUGUI hardText;
    [SerializeField] private TextMeshProUGUI softText;
    
    [SerializeField] private Button startWheelsChanging;
    [SerializeField] private Button finishWheelsChanging;

    private void Awake()
    {
        startWheelsChanging.onClick.AddListener(() => _hangarPresenter.StartWheelsChanging());
        finishWheelsChanging.onClick.AddListener(() => _hangarPresenter.FinishWheelsChanging());
        
        _hangarPresenter.OnHardChanged += OnHardChanged;
        _hangarPresenter.OnSoftChanged += OnSoftChanged;
    }

    private void OnHardChanged(long hard)
    {
        hardText.text = hard.ToString();
    }

    private void OnSoftChanged(long soft)
    {
        softText.text = soft.ToString();
    }

    private void OnDestroy()
    {
        _hangarPresenter.OnHardChanged -= OnHardChanged;
        _hangarPresenter.OnSoftChanged -= OnSoftChanged;
    }
}
