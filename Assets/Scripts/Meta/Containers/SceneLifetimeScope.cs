using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Meta.Containers
{
    public class SceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private SceneContext sceneContext;
    
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(sceneContext).AsSelf();
        }
    }
}
