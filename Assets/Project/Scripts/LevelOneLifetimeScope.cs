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
    [SerializeField] private GameResultController gameResultController;
    [SerializeField] private PauseMenuController pauseMenuController;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(playerMovement);
        builder.RegisterComponent(playerInputReader);
        builder.RegisterComponent(thirdPersonCameraController);
        builder.RegisterComponent(timeCounter);
        builder.RegisterComponent(scoreCounter);
        builder.RegisterComponent(gameResultController);
        builder.RegisterComponent(pauseMenuController);

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
