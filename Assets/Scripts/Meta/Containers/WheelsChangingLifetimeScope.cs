using System.Threading;
using Meta.UseCases;
using Meta.Views;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class WheelsChangingLifetimeScope : LifetimeScope
{
    [SerializeField] private WheelsChangingView wheelsChangingView;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<CancellationToken>(Lifetime.Transient).AsSelf();
        builder.Register<WheelsChangingUseCase>(Lifetime.Scoped).AsImplementedInterfaces();
        builder.Register<WheelsChangingPresenter>(Lifetime.Scoped).AsImplementedInterfaces();
        builder.RegisterComponent(wheelsChangingView);
    }
}
