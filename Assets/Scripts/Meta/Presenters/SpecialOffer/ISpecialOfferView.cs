using System;
using Cysharp.Threading.Tasks;

namespace Meta.Presenters
{
    public interface ISpecialOfferView: IDisposable
    {
        UniTask<bool> GetUserChoice();
    }
}
