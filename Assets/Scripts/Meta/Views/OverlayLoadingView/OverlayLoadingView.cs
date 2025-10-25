using Meta.Presenters;
using UnityEngine;

public class OverlayLoadingView : MonoBehaviour, IOverlayLoadingView
{
    public void ShowOverlayLoading() => gameObject.SetActive(true);
    public void HideOverlayLoading() => gameObject.SetActive(false);
}