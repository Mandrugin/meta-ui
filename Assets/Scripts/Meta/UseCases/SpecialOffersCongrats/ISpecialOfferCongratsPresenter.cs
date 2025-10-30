using System;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface ISpecialOfferCongratsPresenter : IDisposable
    {
        UniTask GetClick();
    }
}
