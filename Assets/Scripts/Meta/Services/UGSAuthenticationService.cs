using Cysharp.Threading.Tasks;
using Meta.UseCases;

namespace Meta.Services
{
    public class UGSAuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated { get; }
        public UniTask<bool> Authenticate()
        {
            throw new System.NotImplementedException();
        }
    }
}