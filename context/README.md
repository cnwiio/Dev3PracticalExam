# Project Context & Agent Guide

Welcome to the context directory for **Dev3PracticalExam** (Unity 6 Game Development Course 958321 — Practical Exam).

This directory provides structured, authoritative documentation specifically curated for **AI coding agents** and human developers working on this codebase. Before proposing or executing architectural changes, new mechanics, or bug fixes, consult the context files listed below.

---

## 🧭 Context Index

| Document | Description |
| :--- | :--- |
| **[GDD.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/GDD.md)** | **Game Design Document (GDD)**: Course examination specification, rules, win/loss criteria, scoring, rubrics, and submission guidelines. |
| **[project-overview.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/project-overview.md)** | Engine version, dependencies (MessagePipe, UniTask, ProBuilder), repository directory structure, and third-party packages. |
| **[architecture.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/architecture.md)** | Dependency injection (VContainer), in-memory messaging (MessagePipe), ScriptableObject stats pattern, scene lifecycles, and pause/time-scale handling. |
| **[systems-breakdown.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/systems-breakdown.md)** | Detailed documentation of Player (Run/Jump/Coyote/Buffer), Cinemachine Third-Person Camera, Stage 1 Minigame, MessagePipe Items, Audio, UI & Settings, and Scene Management. |
| **[scenes-and-prefabs.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/scenes-and-prefabs.md)** | Build scenes (`MainMenu`, `GamePlay`, `Stage2`), prefabs breakdown (`Item/`, `Player/`, `UI/`), and asset organization. |
| **[conventions-and-guidelines.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/conventions-and-guidelines.md)** | C# coding standards, VContainer & MessagePipe rules, ScriptableObject stats workflow, Input System guidelines, physics best practices, and examination rubric compliance. |

---

## 📖 Setup Manuals Reference

The repository also contains dedicated Unity Editor setup manuals in `Manuals/`:
- **[Manuals/Run_Jump_System_Manual.txt](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/Manuals/Run_Jump_System_Manual.txt)**: Rigidbody run/jump tuning, coyote time, and layer masks.
- **[Manuals/Third-Person_Camera_Manual.txt](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/Manuals/Third-Person_Camera_Manual.txt)**: Cinemachine 3 follow camera and `CameraTarget` setup.
- **[Manuals/Win_Lose_System_Manual.txt](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/Manuals/Win_Lose_System_Manual.txt)**: Stage 1 result controller, pitfall threshold, and UI panels.
- **[Manuals/Pause_Menu_Manual.txt](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/Manuals/Pause_Menu_Manual.txt)**: Pause panel, time-scale freezing, and settings modal integration.

---

## ⚡ Quick Project Facts

- **Unity Version:** `6000.3.18f1` (Unity 6 LTS)
- **Render Pipeline:** Universal Render Pipeline (URP `17.3.0`)
- **Level Design:** Unity ProBuilder `6.0.9`
- **Input Solution:** Unity New Input System (`com.unity.inputsystem` `1.19.0`)
- **Dependency Injection:** VContainer (`1.19.0`)
- **Messaging:** MessagePipe (`com.cysharp.messagepipe` + `MessagePipe.VContainer`)
- **Async Utility:** UniTask (`com.cysharp.unitask`)
- **Camera Controller:** Unity Cinemachine `3.1.7` (Orbital Third-Person Follow)
- **UI & Tweening:** TextMeshPro (TMP `2.0.0`) + LeanTween
- **Audio:** Custom `SoundManager` singleton + `AudioMixer` with decibel conversion
- **Primary Code Root:** `Assets/Project/Scripts/`
- **Build Scenes:**
  - `Assets/Project/Scenes/MainMenu.unity` (Build Index `0`)
  - `Assets/Project/Scenes/GamePlay.unity` (Build Index `1` — Stage 1 Capsule Player)
  - `Assets/Project/Scenes/Stage2.unity` (Stage 2 Humanoid Player — in progress)

---

## 🤖 Instructions for AI Agents

1. **Follow Examination GDD**: Review **[context/GDD.md](file:///d:/Work/Unity%20Project/GameDev3_Lab/Dev3PracticalExam/context/GDD.md)** for exact win/lose thresholds, scoring logic, item requirements, and rubric points.
2. **Namespace Rule (Important)**: Per GDD Section 7.1, new custom scripts should follow the exam naming and student namespace guidelines when instructed.
3. **Architecture Rules**:
   - Scene-level services belong in `LevelOneLifetimeScope` (or scene-specific LifetimeScope).
   - Use MessagePipe (`IPublisher<T>` / `ISubscriber<T>`) for cross-cutting gameplay events (such as item pickups).
   - Use ScriptableObjects (`PlayerMovementStats`, `ThirdPersonCameraSettings`) for gameplay values to allow live runtime tuning.
4. **TimeScale Hygiene**: Whenever freezing `Time.timeScale = 0f` (pause, win, lose), always ensure `Time.timeScale = 1f` is restored when loading scenes or resuming.
