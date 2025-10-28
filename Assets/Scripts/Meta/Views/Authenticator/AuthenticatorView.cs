using System;
using Meta.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.Views
{
    public class AuthenticatorView: MonoBehaviour, IAuthenticatorView
    {
        [SerializeField] private Button authButton;
        [SerializeField] private TextMeshProUGUI authInfoText;
        [SerializeField] private GameObject inProgressIcon;
        
        public event Action OnAuthenticate = delegate { };

        private void Awake()
        {
            authButton.onClick.AddListener(() => OnAuthenticate.Invoke());
        }

        public void ShowReadyState()
        {
            authInfoText.gameObject.SetActive(true);
            authInfoText.text = "Authenticate to continue";
            inProgressIcon.gameObject.SetActive(false);
            authButton.gameObject.SetActive(true);
        }

        public void ShowInProgressState()
        {
            authInfoText.gameObject.SetActive(true);
            authInfoText.text = "Authentication in progress...";
            inProgressIcon.gameObject.SetActive(true);
            authButton.gameObject.SetActive(false);
        }

        public void ShowAuthResponse(bool isSuccess)
        {
            authInfoText.gameObject.SetActive(true);
            authInfoText.text = isSuccess ?
                "<color=green>Authentication succeeded</color>"
                : "<color=red>Authentication failed</color>";
            inProgressIcon.gameObject.SetActive(false);
            authButton.gameObject.SetActive(!isSuccess);
        }
    }
}
