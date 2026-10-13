# Agent Instructions & Project Context

Welcome to **Dev3PracticalExam** (Unity 6 / URP / VContainer / MessagePipe / ProBuilder).

This workspace includes a dedicated `context/` directory containing complete architectural documentation, system breakdowns, game design rules, and coding guidelines.

## 📚 Context Directory Reference

Before creating or editing code, consult the following context files:

- **[context/README.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/README.md)**: Master index, quick facts, and setup manuals reference.
- **[context/GDD.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/GDD.md)**: **Game Design Document (GDD)** — Examination rules, 30s Time Attack, win/loss criteria ($>10,000$ pts), and 75-point evaluation rubric.
- **[context/project-overview.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/project-overview.md)**: Engine version (`Unity 6000.3.18f1`), packages (MessagePipe, UniTask, ProBuilder 6, Cinemachine 3), and repository structure.
- **[context/architecture.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/architecture.md)**: VContainer DI (`LevelOneLifetimeScope`), MessagePipe in-memory pub/sub, ScriptableObject stats tuning, and pause/timescale handling.
- **[context/systems-breakdown.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/systems-breakdown.md)**: Deep dive into Player (Run/Jump/Coyote/Buffer), Third-Person Camera, Stage 1 Minigame, MessagePipe Items, Audio, and UI & Settings.
- **[context/scenes-and-prefabs.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/scenes-and-prefabs.md)**: Scenes (`MainMenu`, `GamePlay`, `Stage2`), prefabs breakdown (`Item/`, `Player/`, `UI/`), and audio assets.
- **[context/conventions-and-guidelines.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/conventions-and-guidelines.md)**: C# conventions, VContainer & MessagePipe rules, Unity 6 physics (`rg.linearVelocity`), timescale hygiene, and agent checklist.

## 🛠️ Editor Setup Manuals

Refer to `Manuals/` for Unity Editor component configuration:
- `Manuals/Run_Jump_System_Manual.txt`
- `Manuals/Third-Person_Camera_Manual.txt`
- `Manuals/Win_Lose_System_Manual.txt`
- `Manuals/Pause_Menu_Manual.txt`

## 🚀 Key Rules for AI Agents

1. **Custom Code Path**: Place all new scripts inside `Assets/Project/Scripts/` under appropriate subfolders (`Camera/`, `Player/`, `Stage 1/`, `Sound/`, `UI/`, etc.). Do not touch third-party plugin folders.
2. **Dependency Injection & Messaging**: Use VContainer `LifetimeScope` for scene dependencies and MessagePipe (`IPublisher<T>` / `ISubscriber<T>`) for gameplay event decoupling.
3. **Live Tuning via ScriptableObjects**: Keep tunable physics/camera stats in ScriptableObjects (`PlayerMovementStats`, `ThirdPersonCameraSettings`).
4. **Input Handling**: Use `PlayerInputReader` and `InputActionReference`. Always unsubscribe from callbacks in `OnDisable()`. Use `SetUIMode(true/false)` when pausing.
5. **Physics**: Apply `Rigidbody` forces and velocities (`rg.linearVelocity`) exclusively inside `FixedUpdate()`.
6. **TimeScale Hygiene**: Always restore `Time.timeScale = 1f` before loading scenes or resuming.
7. **Preserve Comments**: Preserve existing code comments (including Thai comments in UI and settings scripts).
