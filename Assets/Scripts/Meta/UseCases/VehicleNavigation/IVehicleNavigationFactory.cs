using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehicleNavigationFactory : IDisposable
    {
        UniTask<IVehicleNavigationPresenter> GetVehicleNavigationPresenter(CancellationToken cancellationToken);
        void DestroyVehicleNavigationPresenter();
    }
}