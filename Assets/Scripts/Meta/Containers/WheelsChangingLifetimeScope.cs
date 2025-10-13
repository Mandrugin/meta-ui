using Meta.Presenters;
using Meta.UseCases;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class WheelsChangingLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<WheelsChangingUseCase>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<WheelsChangingPresenter>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}
