using Meta.Factories;
using Meta.Services;
using Meta.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class InitLifetimeScope : LifetimeScope
    {
        [SerializeField] private string sceneName;
        [SerializeField] private OverlayLoadingFactory overlayLoadingFactory;
        [SerializeField] private AuthenticatorFactory authenticatorFactory;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(overlayLoadingFactory).AsImplementedInterfaces();
            builder.RegisterComponent(authenticatorFactory).AsImplementedInterfaces().AsSelf();
            builder.Register<TestAuthenticationService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<AuthenticatorUseCase>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(sceneName);
        }
    }
}
