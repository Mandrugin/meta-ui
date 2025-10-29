using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }
    
        UniTask<bool> Authenticate();
    }
}