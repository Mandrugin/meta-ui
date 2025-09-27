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
        [SerializeField] private VehiclesPrefabConfig vehiclesPrefabConfig;
        [SerializeField] private WheelsPrefabConfig wheelsPrefabConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<HangarPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterInstance(vehiclesPrefabConfig).AsSelf();
            builder.RegisterInstance(wheelsPrefabConfig).AsSelf();
        }
    }
}
