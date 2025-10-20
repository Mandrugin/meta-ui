using VContainer;
using VContainer.Unity;

namespace Meta.UseCases
{
    public class UseCaseLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<UseCaseMediator>(Lifetime.Singleton).AsSelf();
        }
    }
}