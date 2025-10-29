using Meta.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class BasicLifetimeScope : LifetimeScope
    {
        [SerializeField] private OverlayLoadingFactory overlayLoadingFactory;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(overlayLoadingFactory).AsImplementedInterfaces();
        }
    }
}
