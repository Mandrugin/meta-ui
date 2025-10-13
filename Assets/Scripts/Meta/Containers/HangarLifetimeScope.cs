using Meta.Configs;
using Meta.Gateway;
using Meta.Presenters;
using Meta.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class HangarLifetimeScope : LifetimeScope
    {
        [SerializeField] private VehiclesDataConfig vehiclesDataConfig;
        [SerializeField] private WheelsDataConfig wheelsDataConfig;

        [SerializeField] private VehiclesViewConfig vehiclesViewConfig;
        [SerializeField] private WheelsViewConfig wheelsViewConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<HangarPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<VehiclePresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterInstance(vehiclesDataConfig).AsSelf();
            builder.RegisterInstance(wheelsDataConfig).AsSelf();

            builder.RegisterInstance(vehiclesViewConfig).AsSelf();
            builder.RegisterInstance(wheelsViewConfig).AsSelf();
        }
    }
}
