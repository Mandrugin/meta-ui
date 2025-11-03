using Cysharp.Threading.Tasks;
using Meta.UseCases;
using UnityEngine.Scripting;

namespace Meta.Services
{
    [Preserve]
    public class UgsAuthenticatorService : IAuthenticatorService
    {
        public bool IsAuthenticated => _ugsAuthenticationService.IsAuthenticated();
        
        private readonly UgsPlayerAccountService _ugsPlayerAccountService = new();
        private readonly UgsAuthenticationService _ugsAuthenticationService = new();

        public async UniTask InitializeAsync()
        {
            await _ugsPlayerAccountService.InitializeAsync();
            await _ugsAuthenticationService.InitializeAsync();

            if (_ugsAuthenticationService.HasSessionToken())
            {
                await _ugsAuthenticationService.SignInAnonymouslyAsync();
                return;
            }

            await _ugsPlayerAccountService.SignInAsync();
        }

        public UniTask<bool> Authenticate()
        {
            return _ugsAuthenticationService.SignInWithUnityAsync(_ugsPlayerAccountService.GetToken());
        }

        public void Dispose()
        {
            _ugsPlayerAccountService.Dispose();
        }
    }
}