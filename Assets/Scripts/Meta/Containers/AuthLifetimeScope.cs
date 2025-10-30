using Meta.Factories;
using Meta.Services;
using Meta.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class AuthLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameObject hangarScope;
        [SerializeField] private GameObject specialOffersScope;

        [SerializeField] private AuthenticatorFactory authenticatorFactory;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(authenticatorFactory).AsImplementedInterfaces().AsSelf();
            builder.RegisterInstance(hangarScope).AsSelf().Keyed(ScopeKeys.HangarScope);
            builder.RegisterInstance(specialOffersScope).AsSelf().Keyed(ScopeKeys.SpecialOffersScope);
            builder.Register<AuthenticatorUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UGSAuthenticationService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
