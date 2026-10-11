# Coding Conventions & Agent Guidelines

This document outlines the coding standards, patterns, and critical guidelines for any AI agent or developer modifying or extending the **Dev3PracticalExam** project.

---

## 1. Code Placement & File Structure

- **All Custom Code Belongs in `Assets/Project/Scripts/`**:
  - Subsystems must be grouped by domain (e.g., `Player/`, `Sound/`, `UI/`, `Core/`).
  - **Never** add custom scripts directly inside `Assets/Plugins/`, `Assets/Sortify/`, `Assets/OpalStudio/`, or `Assets/LeanTween/`.
- **Match Script Name with Class Name**: C# requires MonoBehaviours to have the exact same filename as the class.
- **Unity `.meta` Files**: Unity tracks assets using GUIDs in `.meta` files. If you create or rename C# files or assets, ensure Unity can generate or keep associated `.meta` files intact.

---

## 2. C# Coding Conventions

- **Naming:**
  - `PascalCase`: Classes, Structs, Enums, Interfaces (`ISomething`), Public Methods, Properties, Public Fields.
  - `camelCase`: Private/Protected fields, local variables, parameters.
- **Encapsulation:**
  - Avoid public fields for inspector access. Use `[SerializeField] private Type fieldName;`.
  - Expose state through properties with private or protected setters:
    ```csharp
    public Vector2 MovementInput { get; private set; }
    ```
- **Docstrings & Comments:**
  - Preserve all existing comments (including Thai comments in `GameSettingsManager.cs` and `ClickableText.cs`).
  - Document public APIs and architectural decisions clearly.

---

## 3. Dependency Injection (VContainer) Guidelines

1. **Register in LifetimeScopes**:
   - When adding a new scene-level service or MonoBehaviour that needs to be consumed across scripts, add it to `LevelOneLifetimeScope` (or create a dedicated scope).
   - Use `builder.RegisterComponent(myComponent);` for scene MonoBehaviours, or `builder.Register<MyService>(Lifetime.Scoped);` for pure C# classes.
2. **Method Injection for MonoBehaviours**:
   - Unity instantiates MonoBehaviours, so constructor injection cannot be used. Use method injection with `[Inject]`:
     ```csharp
     [Inject]
     public void Construct(IDependency dependency)
     {
         _dependency = dependency;
     }
     ```
3. **Avoid Service Locator Anti-Patterns**:
   - Avoid `FindObjectOfType`, `FindAnyObjectByType`, or `GameObject.Find` inside core gameplay loops.
   - Inject dependencies through VContainer scopes.

---

## 4. Unity New Input System Guidelines

1. **Decouple Input from Physics**:
   - Input polling/callbacks should occur in input reader scripts (`PlayerInputReader`).
   - Gameplay scripts (`PlayerMovement`) should read processed properties (e.g. `Vector2 MovementInput`), not raw hardware inputs.
2. **Always Unsubscribe in `OnDisable`**:
   - If you subscribe to `InputAction.performed` or `canceled` in `OnEnable()`, **always** unsubscribe in `OnDisable()` to prevent memory leaks and exceptions.
     ```csharp
     private void OnEnable()
     {
         moveAction.action.performed += OnMove;
         moveAction.action.canceled += OnStopMove;
     }

     private void OnDisable()
     {
         moveAction.action.performed -= OnMove;
         moveAction.action.canceled -= OnStopMove;
     }
     ```
3. **Action Map Management**:
   - Use `InputManager` to switch between `Player` and `UI` action maps to prevent character movement while navigating menus.

---

## 5. Physics & Movement Guidelines

1. **Use `FixedUpdate` for Physics**:
   - When applying forces (`rg.AddForce`) or setting velocities on a `Rigidbody`, execute calculations inside `FixedUpdate()` rather than `Update()` to ensure deterministic simulation across varied framerates.
2. **Guard Against Zero Division / Zero Normalization**:
   - When calculating normalized direction vectors:
     ```csharp
     Vector3 dir = new Vector3(input.x, 0, input.y);
     if (dir.sqrMagnitude > 0.001f)
     {
         rg.AddForce(dir.normalized * speed, ForceMode.Force);
     }
     ```

---

## 6. Audio Conventions

1. **Decibel Scale Conversion**:
   - Unity's `AudioMixer` volume uses decibels (`-80dB` to `0dB` or `+20dB`).
   - When converting linear 0.0–1.0 values, clamp the minimum to `0.0001f` before calling `Mathf.Log10`:
     ```csharp
     float dB = Mathf.Log10(Mathf.Clamp(linearLevel, 0.0001f, 1f)) * 20f;
     audioMixer.SetFloat("ParamName", dB);
     ```
2. **SFX vs Music**:
   - Music tracks: Use `SoundManager.Instance.PlayBGM("TrackName")`.
   - Transient effects: Use `SoundManager.Instance.PlaySFX("SFXName")`.

---

## 7. AI Agent Checklist (Before Submitting Changes)

- [ ] Does the new script live in `Assets/Project/Scripts/`?
- [ ] If it is a scene dependency, is it registered in the relevant `LifetimeScope`?
- [ ] Are input subscriptions balanced between `OnEnable` and `OnDisable`?
- [ ] Are Rigidbody forces calculated in `FixedUpdate`?
- [ ] Are existing comments and code formatting preserved?
- [ ] Have compile errors been checked?
