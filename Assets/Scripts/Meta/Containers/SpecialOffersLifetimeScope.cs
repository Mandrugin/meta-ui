using Meta.DataConfigs;
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
        [SerializeField] private SpecialOfferFactory specialOfferFactory;
        [SerializeField] private SpecialOffersCongratsFactory specialOffersCongratsFactory;
        [SerializeField] private SpecialOffersDataConfig specialOffersDataConfig;
        [SerializeField] private ProfileDataConfig profileDataConfig;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(specialOffersFactory).AsImplementedInterfaces();
            builder.RegisterComponent(specialOfferFactory).AsImplementedInterfaces();
            builder.RegisterComponent(specialOffersCongratsFactory).AsImplementedInterfaces();
            builder.RegisterComponent(specialOffersDataConfig).AsSelf();
            builder.RegisterComponent(profileDataConfig).AsSelf();
            builder.Register<TestSpecialOffersService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SpecialOffersUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
