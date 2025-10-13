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
        [SerializeField] private ViewVehiclesConfig viewVehiclesConfig;
        [SerializeField] private ViewWheelsConfig viewWheelsConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<HangarPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<VehiclePresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterInstance(viewVehiclesConfig).AsSelf();
            builder.RegisterInstance(viewWheelsConfig).AsSelf();
        }
    }
}
