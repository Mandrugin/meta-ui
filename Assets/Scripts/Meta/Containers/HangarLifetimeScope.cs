using Meta.Gateway;
using Meta.Presenters;
using Meta.UseCases;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class HangarLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LocalHangarGateway>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<HangarUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<HangarPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
