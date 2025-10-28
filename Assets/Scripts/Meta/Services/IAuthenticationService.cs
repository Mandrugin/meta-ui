using Cysharp.Threading.Tasks;

namespace Meta.Services
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }
    
        UniTask<bool> Authenticate();
    }
}