using System;
using Cysharp.Threading.Tasks;

namespace Meta.Presenters
{
    public interface ISpecialOffersCongratsView: IDisposable
    {
        UniTask GetClick();
    }
}
