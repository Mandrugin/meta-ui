using Meta.UseCases;
using VContainer;
using VContainer.Unity;

public class HangarLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<LocalHangarBackend>(Lifetime.Scoped).AsImplementedInterfaces();
        builder.Register<HangarUseCase>(Lifetime.Scoped).AsImplementedInterfaces();
    }
}
