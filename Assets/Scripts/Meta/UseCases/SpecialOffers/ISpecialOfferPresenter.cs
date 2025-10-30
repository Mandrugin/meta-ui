using System;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOfferPresenter : IDisposable
    {
        UniTask<bool> GetUserChoice();
    }
}
