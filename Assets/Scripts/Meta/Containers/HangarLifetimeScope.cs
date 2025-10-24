using Meta.DataConfigs;
using Meta.Factories;
using Meta.Gateways;
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

        [SerializeField] private HangarFactory hangarFactory;
        [SerializeField] private VehicleNavigationFactory vehicleNavigationFactory;
        [SerializeField] private VehicleFactory vehicleFactory;
        [SerializeField] private WheelsChangingFactory wheelsChangingFactory;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponent(hangarFactory).AsImplementedInterfaces().AsSelf();

            builder.Register<VehicleUseCase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponent(vehicleFactory).AsImplementedInterfaces();
            
            builder.Register<VehicleNavigationUseCase>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponent(vehicleNavigationFactory).AsImplementedInterfaces();

            builder.Register<WheelsChangingUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterComponent(wheelsChangingFactory).AsImplementedInterfaces();


            builder.RegisterInstance(vehiclesDataConfig).AsSelf();
            builder.RegisterInstance(wheelsDataConfig).AsSelf();
            
            builder.RegisterInstance(profileDataConfig).AsSelf();

            builder.RegisterInstance(vehiclesViewConfig).AsSelf();
            builder.RegisterInstance(wheelsViewConfig).AsSelf();
        }
    }
}
