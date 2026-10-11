using MessagePipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelOneLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private ThirdPersonCameraController thirdPersonCameraController;
    [SerializeField] private TimeCounter timeCounter;
    [SerializeField] private ScoreCounter scoreCounter;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(playerMovement);
        builder.RegisterComponent(playerInputReader);
        builder.RegisterComponent(thirdPersonCameraController);
        builder.RegisterComponent(timeCounter);
        builder.RegisterComponent(scoreCounter);

        // MessagePipe: item interaction events
        MessagePipeOptions options = builder.RegisterMessagePipe();
        builder.RegisterMessageBroker<ItemCollectedMessage>(options);

        builder.Register<ScoreManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<ItemSfxPlayer>();

        // Inject every Item placed in the scene (items are not registered individually)
        builder.RegisterBuildCallback(container =>
        {
            foreach (Item item in FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                container.Inject(item);
            }
        });
    }
}
