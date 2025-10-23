using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IVehicleFactory
    {
        UniTask<IVehiclePresenter> GetVehiclePresenter(CancellationToken cancellationToken);
        UniTask<IVehicleNavigationPresenter> GetVehicleNavigationPresenter(CancellationToken cancellationToken);
        void DestroyVehiclePresenter();
        void DestroyVehicleNavigationPresenter();
    }
}