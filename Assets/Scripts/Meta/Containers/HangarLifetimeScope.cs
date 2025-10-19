using Meta.DataConfigs;
using Meta.Gateways;
using Meta.Presenters;
using Meta.UseCases;
using Meta.ViewConfigs;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class HangarLifetimeScope : LifetimeScope
    {
        [SerializeField] private VehiclesDataConfig vehiclesDataConfig;
        [SerializeField] private WheelsDataConfig wheelsDataConfig;
        
        [SerializeField] private ProfileDataConfig profileDataConfig;

        [SerializeField] private VehiclesViewConfig vehiclesViewConfig;
        [SerializeField] private WheelsViewConfig wheelsViewConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<VehicleUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WheelsChangingUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WheelsChangingPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<HangarPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<VehiclePresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterInstance(vehiclesDataConfig).AsSelf();
            builder.RegisterInstance(wheelsDataConfig).AsSelf();
            
            builder.RegisterInstance(profileDataConfig).AsSelf();

            builder.RegisterInstance(vehiclesViewConfig).AsSelf();
            builder.RegisterInstance(wheelsViewConfig).AsSelf();
        }
    }
}
