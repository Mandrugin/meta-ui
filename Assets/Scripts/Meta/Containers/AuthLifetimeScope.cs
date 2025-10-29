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

        [SerializeField] private AuthenticatorFactory authenticatorFactory;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(authenticatorFactory).AsImplementedInterfaces().AsSelf();
            builder.Register<AuthenticatorUseCase>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(hangarScope);
            builder.Register<TestAuthenticationService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}
