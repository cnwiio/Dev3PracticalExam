# Coding Conventions & Agent Guidelines

This document outlines the coding standards, architecture patterns, and critical guidelines for any AI agent or developer extending the **Dev3PracticalExam** project.

---

## 1. Code Placement & Namespace Standards

- **All Custom Code Belongs in `Assets/Project/Scripts/`**:
  - Subsystems must be grouped by domain (e.g., `Camera/`, `Player/`, `Sound/`, `Stage 1/`, `Stage 1/Items/`).
  - **Never** place custom scripts inside third-party directories (`Assets/Plugins/`, `Assets/Sortify/`, `Assets/OpalStudio/`, `Assets/LeanTween/`).
- **Examination Namespace Rule (GDD 7.1)**:
  - When authoring examination scripts, wrap code in the student's designated namespace (e.g., `namespace StudentName { ... }`).
- **File & Class Naming**:
  - The MonoBehaviour class name must strictly match its filename.
  - Unity `.meta` files must be preserved when renaming or moving assets.

---

## 2. Dependency Injection & MessagePipe Guidelines

1. **VContainer Scopes**:
   - Register scene MonoBehaviours in `LifetimeScope.Configure(...)` using `builder.RegisterComponent(component)`.
   - Register pure C# services with `builder.Register<T>(Lifetime.Singleton)` or `builder.RegisterEntryPoint<T>()`.
2. **Method Injection**:
   - For MonoBehaviours, always use `[Inject] public void Construct(...)`. Do **not** use constructor injection on MonoBehaviours.
   - Avoid `FindObjectOfType` / `FindAnyObjectByType` in core loops.
3. **MessagePipe Pub/Sub**:
   - Register brokers in the LifetimeScope:
     ```csharp
     MessagePipeOptions options = builder.RegisterMessagePipe();
     builder.RegisterMessageBroker<ItemCollectedMessage>(options);
     ```
   - Producers inject `IPublisher<T>` and invoke `publisher.Publish(payload)`.
   - Consumers inject `ISubscriber<T>`, store the `IDisposable` subscription token, and dispose it in `OnDestroy()` or `Dispose()`.
4. **Dynamic Item Injection**:
   - For dynamically placed or prefabs spawned in the scene that require injection, use `builder.RegisterBuildCallback`:
     ```csharp
     builder.RegisterBuildCallback(container =>
     {
         foreach (var item in FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.None))
         {
             container.Inject(item);
         }
     });
     ```

---

## 3. ScriptableObject Stats Pattern

- Parameter collections intended for live tuning should be encapsulated in a `ScriptableObject` (e.g., `PlayerMovementStats`, `ThirdPersonCameraSettings`):
  ```csharp
  [CreateAssetMenu(fileName = "MyStats", menuName = "Project/My Stats")]
  public class MyStats : ScriptableObject { ... }
  ```
- **Live Tuning Advantage:** Values edited in the Inspector while in Play mode take effect immediately and are saved to the asset upon exiting Play mode.

---

## 4. Unity New Input System Guidelines

1. **Decouple Input from Game Logic**:
   - Hardware events belong strictly in input reader components (`PlayerInputReader.cs`).
   - Movement controllers poll clean properties (`MovementInput`, `LookInput`, `RunHeld`, `JumpHeld`).
2. **Action Map Switching (`SetUIMode`)**:
   - Use `PlayerInputReader.SetUIMode(true)` when opening menus. This disables the `Player` action map, zeroes out cached movement/look values, and enables the `UI` action map.
3. **Lifecycle Unsubscriptions**:
   - Always unsubscribe from `performed` and `canceled` delegates in `OnDisable()` to prevent memory leaks and ghost calls.

---

## 5. Physics & Movement Guidelines

1. **Use `FixedUpdate` Exclusively for Rigidbody Physics**:
   - All ground checking, force applications, velocity assignments, and rotation slerps must run inside `FixedUpdate()`.
2. **Unity 6 API Compliance**:
   - Use `rg.linearVelocity` instead of the deprecated `rg.velocity`.
3. **Platformer Feel Techniques**:
   - **Coyote Time:** Allow jumps for `coyoteTime` (e.g. `0.1s`) after `lastGroundedTime`.
   - **Jump Buffer:** Remember jump presses for `jumpBufferTime` (e.g. `0.1s`) before landing.
   - **Variable Jump Height:** Apply higher gravity (`lowJumpGravityMultiplier`) when jump is released before apex.
   - **Ground Check:** Use `Physics.SphereCast` with a dedicated ground layer mask, ensuring the Player's own layer is excluded.

---

## 6. TimeScale & State Hygiene

1. **Pausing and Freezing**:
   - Setting `Time.timeScale = 0f` freezes physics and timers, but ensure `Cursor.lockState = CursorLockMode.None` and `Cursor.visible = true` are applied.
2. **Guaranteed Unfreeze**:
   - Before calling `SceneManager.LoadScene(...)` in `Retry()` or `MainMenu()`, **always** reset `Time.timeScale = 1f`.
   - In `Awake()` or `Start()` of result controllers, ensure `Time.timeScale = 1f` as a safety guard.

---

## 7. Audio Conventions

1. **Decibel Logarithmic Conversion**:
   - Convert linear 0.0–1.0 volume levels to `AudioMixer` decibels:
     ```csharp
     float dB = Mathf.Log10(Mathf.Clamp(linearLevel, 0.0001f, 1f)) * 20f;
     audioMixer.SetFloat(paramName, dB);
     ```
2. **SFX Naming Consistency**:
   - Use established sound names defined in `SoundManager.sfxSounds`: `"ItemCollect"`, `"ItemHit"`, `"Click"`, `"Hover"`, `"Win"`, `"Lose"`.

---

## 8. AI Agent Checklist (Before Submitting Changes)

- [ ] Does the new script reside in `Assets/Project/Scripts/`?
- [ ] If it is a scene dependency, is it registered in `LevelOneLifetimeScope`?
- [ ] If using MessagePipe, are subscriptions disposed in `OnDestroy()` / `Dispose()`?
- [ ] Are Rigidbody forces and velocities calculated in `FixedUpdate()`?
- [ ] Is `rg.linearVelocity` used instead of `rg.velocity`?
- [ ] Does `Time.timeScale` reset to `1f` before scene transitions?
- [ ] Are existing code comments and UTF-8 encoding preserved?
- [ ] Does the win condition strictly check `score > 10000` per GDD specifications?
