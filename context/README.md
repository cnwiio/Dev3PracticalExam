# Project Context & Agent Guide

Welcome to the context directory for **Dev3PracticalExam** (Unity 6 Game Development Project).

This directory provides structured, authoritative documentation specifically curated for **AI coding agents** and human developers working on this codebase. Before proposing or executing architectural changes, new mechanics, or bug fixes, consult the context files listed below.

---

## 🧭 Context Index

| Document | Description |
| :--- | :--- |
| **[project-overview.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/project-overview.md)** | Engine version, dependencies, repository directory structure, and third-party packages. |
| **[architecture.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/architecture.md)** | Dependency injection (VContainer), architecture patterns, scene lifecycles, and cross-cutting concerns. |
| **[systems-breakdown.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/systems-breakdown.md)** | Detailed documentation of Player, Input, Audio, UI & Settings, and Scene Management systems. |
| **[scenes-and-prefabs.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/scenes-and-prefabs.md)** | Build scenes, scene hierarchies, prefabs (`Setting Panel`, `Sound Manager`), and asset organization. |
| **[conventions-and-guidelines.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/conventions-and-guidelines.md)** | C# coding standards, VContainer rules, Input System guidelines, physics practices, and agent dos & don'ts. |

---

## ⚡ Quick Project Facts

- **Unity Version:** `6000.3.18f1` (Unity 6)
- **Render Pipeline:** Universal Render Pipeline (URP `17.3.0`)
- **Input Solution:** Unity New Input System (`com.unity.inputsystem` `1.19.0`)
- **Dependency Injection:** VContainer (`1.19.0`)
- **UI & Tweening:** TextMeshPro (TMP `2.0.0`) + LeanTween
- **Audio:** Custom `SoundManager` singleton + `AudioMixer` with decibel conversion
- **Primary Code Root:** `Assets/Project/Scripts/`
- **Active Scenes:**
  - `Assets/Project/Scenes/MainMenu.unity` (Build Index `0`)
  - `Assets/Project/Scenes/GamePlay.unity` (Build Index `1`)

---

## 🤖 Instructions for AI Agents

1. **Locate Code First**: All project-specific gameplay, managers, and UI code reside in `Assets/Project/Scripts/`. Do **not** place custom code in third-party directories (`Assets/Plugins/`, `Assets/Sortify/`, `Assets/OpalStudio/`).
2. **Respect DI Patterns**: Scene-level dependencies (Player, Input Readers) should be wired through VContainer `LifetimeScope` rather than relying on `FindObjectOfType` or tight coupling.
3. **Preserve Comments & Encoding**: Some existing scripts contain Thai language comments. Always preserve existing comments and ensure UTF-8 encoding is used.
4. **Input Conventions**: Action maps are managed via `InputManager.cs` and read by `PlayerInputReader.cs`. Use `InputActionReference` subscriptions with proper `OnEnable`/`OnDisable` lifecycle handling.
