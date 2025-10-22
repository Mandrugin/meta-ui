
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Meta.UseCases
{
    public interface IHangarFactory
    {
        
    }

    public interface IVehicleFactory
    {
        UniTask<IVehiclePresenter> GetVehiclePresenter(CancellationToken cancellationToken);
        UniTask<IVehicleNavigationPresenter> GetVehicleNavigationPresenter(CancellationToken cancellationToken);
        void DestroyVehiclePresenter();
        void DestroyVehicleNavigationPresenter();
    }
    public interface IWheelsChangingFactory
    { 

    }
}
