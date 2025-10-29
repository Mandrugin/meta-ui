using Meta.Factories;
using Meta.Services;
using Meta.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class SpecialOffersLifetimeScope : LifetimeScope
    {
        [SerializeField] private SpecialOffersFactory  specialOffersFactory;
        [SerializeField] private SpecialOffersDataConfig specialOffersDataConfig;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(specialOffersFactory).AsImplementedInterfaces();
            builder.RegisterComponent(specialOffersDataConfig).AsSelf();
            builder.Register<TestSpecialOffersService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SpecialOffersUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
