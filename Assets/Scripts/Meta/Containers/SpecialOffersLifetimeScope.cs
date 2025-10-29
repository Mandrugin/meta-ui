using Meta.Factories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SpecialOffersLifetimeScope : LifetimeScope
{
    [SerializeField] private SpecialOffersFactory  specialOffersFactory;
    
    protected override void Configure(IContainerBuilder builder)
    {
    }
}
