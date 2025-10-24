using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehicleFactory : IDisposable
    {
        UniTask<IVehiclePresenter> GetVehiclePresenter(CancellationToken cancellationToken);
        void DestroyVehiclePresenter();
    }
}