using Cysharp.Threading.Tasks;

namespace Meta.Services
{
    public class TestAuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated => false;
    
        public async UniTask<bool> Authenticate()
        {
            await UniTask.WaitForSeconds(3);
            return UnityEngine.Random.Range(0, 10) >= 3; // 30% success authentication
        }
    }
}