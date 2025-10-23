using System;
using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HangarView : MonoBehaviour, IHangarView
{

    [SerializeField] private TextMeshProUGUI hardText;
    [SerializeField] private TextMeshProUGUI softText;
    
    [SerializeField] private Button startWheelsChanging;
    [SerializeField] private Button finishWheelsChanging;
    public event Action OnShowWheelsChanging = delegate { };
    public event Action OnHideWheelsChanging = delegate { };

    public void Awake()
    {        
        startWheelsChanging.onClick.AddListener(InvokeOnShowWheelsChanging);
        finishWheelsChanging.onClick.AddListener(InvokeOnHideWheelsChanging);        
    }

    private void InvokeOnShowWheelsChanging() => OnShowWheelsChanging.Invoke();
    private void InvokeOnHideWheelsChanging() => OnHideWheelsChanging.Invoke();

    public void ChangeHard(long hard) => hardText.text = hard.ToString();
    public void ChangeSoft(long soft) => softText.text = soft.ToString();
}
