using System.Threading;
using Meta.Presenters;
using Meta.UseCases;
using VContainer;
using VContainer.Unity;

public class WheelsChangingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<CancellationToken>(Lifetime.Transient).AsSelf();
        builder.Register<WheelsChangingUseCase>(Lifetime.Scoped).AsImplementedInterfaces();
        builder.Register<WheelsChangingPresenter>(Lifetime.Scoped).AsImplementedInterfaces();
    }
}
