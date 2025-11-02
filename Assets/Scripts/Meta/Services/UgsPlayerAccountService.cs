using Cysharp.Threading.Tasks;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;

namespace Meta.Services
{
    public class UgsPlayerAccountService
    {
        private UniTaskCompletionSource<bool> _completionSource;

        public async UniTask InitializeAsync()
        {
            await UnityServices.InitializeAsync();
            
            PlayerAccountService.Instance.SignedIn += OnSignedIn;
            PlayerAccountService.Instance.SignedOut += OnSignedOut;
            PlayerAccountService.Instance.SignInFailed += OnSignInFailed;
        }

        public void Dispose()
        {
            PlayerAccountService.Instance.SignedIn -= OnSignedIn;
            PlayerAccountService.Instance.SignedOut -= OnSignedOut;
            PlayerAccountService.Instance.SignInFailed -= OnSignInFailed;
        }
        
        private void OnSignedIn()
        {
            Debug.Log("PlayerAccountService.Instance.SignedIn");
            _completionSource.TrySetResult(true);
        }

        private void OnSignedOut()
        {
            Debug.Log("PlayerAccountService.Instance.SignedOut");
            _completionSource.TrySetResult(true);
        }

        private void OnSignInFailed(RequestFailedException requestFailedException)
        {
            Debug.LogException(requestFailedException);
            _completionSource.TrySetResult(false);
        }
        
        public UniTask<bool> SignInAsync()
        {
            _completionSource = new UniTaskCompletionSource<bool>();

            if (PlayerAccountService.Instance.IsSignedIn)
            {
                return UniTask.FromResult(true);
            }

            try
            {
                PlayerAccountService.Instance.StartSignInAsync();
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                _completionSource.TrySetResult(false);
            }

            return _completionSource.Task;
        }

        public string GetToken()
            => PlayerAccountService.Instance.AccessToken;
    }
}