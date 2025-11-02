using System;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IAuthenticatorService : IDisposable
    {
        bool IsAuthenticated { get; }
    
        UniTask InitializeAsync();
        UniTask<bool> Authenticate();
    }
}