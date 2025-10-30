using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;
using IAuthenticationService = Meta.UseCases.IAuthenticationService;

namespace Meta.Services
{
    public class UGSAuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated => AuthenticationService.Instance.IsSignedIn;
        
        private readonly UGSPlayerAccountService _playerAccountService = new();

        public async UniTask InitializeAsync()
        {
            await _playerAccountService.InitializeAsync();
            await _playerAccountService.SignInAsync();
        }

        public async UniTask<bool> Authenticate()
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public void Dispose()
        {
            _playerAccountService.Dispose();
        }
    }
}