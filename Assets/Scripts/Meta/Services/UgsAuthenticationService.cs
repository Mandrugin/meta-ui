using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Meta.Services
{
    public class UgsAuthenticationService
    {
        private IAuthenticationService _authenticationService;
        
        public async UniTask InitializeAsync()
        {
            if (AuthenticationService.Instance == null)
                await UnityServices.InitializeAsync();

            _authenticationService = AuthenticationService.Instance;
        }

        public bool IsAuthenticated()
        {
            try
            {
                if(_authenticationService == null)
                    throw new NullReferenceException("UgsAuthenticationService has not been initialized");

                return _authenticationService.IsAuthorized;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public bool HasSessionToken()
        {
            try
            {
                if(_authenticationService == null)
                    throw new NullReferenceException("UgsAuthenticationService has not been initialized");

                return _authenticationService.SessionTokenExists;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        public async UniTask SignInAnonymouslyAsync()
        {
            try
            {
                if(_authenticationService == null)
                    throw new NullReferenceException("UgsAuthenticationService has not been initialized");

                await _authenticationService.SignInAnonymouslyAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        public async UniTask<bool> SignInWithUnityAsync(string token)
        {
            try
            {
                if(_authenticationService == null)
                    throw new NullReferenceException("UgsAuthenticationService has not been initialized");

                await _authenticationService.SignInWithUnityAsync(token);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }
    }
}