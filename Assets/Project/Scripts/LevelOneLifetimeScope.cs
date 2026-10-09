using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelOneLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputReader playerInputReader;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(playerMovement);
        builder.RegisterComponent(playerInputReader);
    }
}
