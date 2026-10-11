# Agent Instructions & Project Context

Welcome to **Dev3PracticalExam** (Unity 6 / URP / VContainer).

This workspace includes a dedicated `context/` directory containing complete architectural documentation, system breakdowns, and coding guidelines.

## 📚 Context Directory Reference

Before creating or editing code, consult the following context files:

- **[context/README.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/README.md)**: Master index and quick start summary.
- **[context/project-overview.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/project-overview.md)**: Engine version (`Unity 6000.3.18f1`), URP, packages, and directory breakdown.
- **[context/architecture.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/architecture.md)**: VContainer Dependency Injection (`LevelOneLifetimeScope`), singleton patterns, and event flows.
- **[context/systems-breakdown.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/systems-breakdown.md)**: Deep dive into Player, Input System, Audio Subsystem, Settings & UI.
- **[context/scenes-and-prefabs.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/scenes-and-prefabs.md)**: Build scenes (`MainMenu`, `GamePlay`), prefabs, and assets.
- **[context/conventions-and-guidelines.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/conventions-and-guidelines.md)**: C# conventions, VContainer rules, Input lifecycle, and agent checklist.

## 🚀 Key Rules for AI Agents

1. **Custom Code Path**: Place all new scripts inside `Assets/Project/Scripts/` under appropriate subfolders (`Player/`, `Sound/`, `UI/`, etc.). Do not touch third-party plugin folders.
2. **Dependency Injection**: Use VContainer `LifetimeScope` for scene dependencies with `[Inject] public void Construct(...)` method injection. Avoid `FindObjectOfType` in core loops.
3. **Input Handling**: Use `PlayerInputReader` and `InputActionReference`. Always unsubscribe from action callbacks in `OnDisable()`.
4. **Physics**: Apply `Rigidbody` forces inside `FixedUpdate()`.
5. **Preserve Comments**: Preserve existing code comments (including Thai comments in UI and settings scripts).
