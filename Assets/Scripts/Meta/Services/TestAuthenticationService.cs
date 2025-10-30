using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Services
{
    public class TestAuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated => false;

        public async UniTask InitializeAsync()
        {
            await UniTask.WaitForSeconds(1);
        }

        public async UniTask<bool> Authenticate()
        {
            await UniTask.WaitForSeconds(3);
            var response = UnityEngine.Random.Range(0, 10) >= 3; // 30% success authentication
            if (response)
                await UniTask.WaitForSeconds(1);
            return response;
        }

        public void Dispose()
        {
            // ...
        }
    }
}
