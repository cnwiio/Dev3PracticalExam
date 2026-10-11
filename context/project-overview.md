# Project Overview & Tech Stack

This document details the high-level architecture, technology stack, directory organization, and package dependencies for the **Dev3PracticalExam** project.

---

## 1. Engine & Environment

- **Unity Editor Version:** `6000.3.18f1` (Unity 6 LTS)
- **Target Platform:** Desktop / PC (Windows)
- **Render Pipeline:** Universal Render Pipeline (URP `17.3.0`)
- **Scripting Backend / .NET:** .NET Standard 2.1 / C# 9.0+

---

## 2. Key Package Dependencies

The project leverages official Unity packages and selected community frameworks via `Packages/manifest.json`:

| Package | Version | Purpose |
| :--- | :--- | :--- |
| `jp.hadashikick.vcontainer` | `1.19.0` (Git) | Lightweight, high-performance Dependency Injection framework for Unity. |
| `com.unity.inputsystem` | `1.19.0` | Event-driven Unity New Input System replacing legacy Input manager. |
| `com.unity.render-pipelines.universal` | `17.3.0` | Universal Render Pipeline (URP) graphics shaders and render passes. |
| `com.unity.cinemachine` | `3.1.7` | Camera control, procedural follow, look-at and virtual camera management. |
| `com.unity.ai.navigation` | `2.0.13` | NavMesh surface generation and AI pathfinding. |
| `com.unity.ugui` | `2.0.0` | Unity UI (Canvas, TextMeshPro, EventSystem). |
| `com.unity.timeline` | `1.8.12` | Cutscenes, sequencing, and multi-track animation. |

---

## 3. Directory Structure

The repository is structured with a clear separation between project-specific game logic and vendor/third-party plugins:

```
Dev3PracticalExam/
├── Assets/
│   ├── InputSystem_Actions.inputactions   # Root InputActionAsset (Player, UI, etc.)
│   ├── Project/                          # PRIMARY GAME CODE & ASSETS
│   │   ├── Assets/                       # Art, 2D sprites, animations, fonts
│   │   │   ├── 2D/
│   │   │   ├── Animation/
│   │   │   ├── Character/
│   │   │   └── Fonts/
│   │   ├── Prefabs/                      # Reusable GameObject templates
│   │   │   ├── Setting Panel.prefab      # Graphic & audio settings modal UI
│   │   │   └── Sound Manager.prefab      # Persistent audio manager prefab
│   │   ├── Scenes/                       # Active game scenes
│   │   │   ├── MainMenu.unity            # Start menu scene (Build index 0)
│   │   │   ├── GamePlay.unity            # Main game level (Build index 1)
│   │   │   └── SampleScene.unity         # Scratch / default scene (disabled in build)
│   │   ├── Scripts/                      # Custom C# gameplay and architecture code
│   │   │   ├── ClickableText.cs          # Interactive text UI component with LeanTween
│   │   │   ├── GameSettingsManager.cs    # Graphics/audio options & PlayerPrefs manager
│   │   │   ├── LevelOneLifetimeScope.cs  # VContainer DI scope for GamePlay scene
│   │   │   ├── MainMenuLifetimeScope.cs  # VContainer DI scope for MainMenu scene
│   │   │   ├── ScenesManager.cs          # Scene loading & app quit handling
│   │   │   ├── Player/                   # Player subsystem
│   │   │   │   ├── InputManager.cs       # ActionMap switcher (Player vs UI)
│   │   │   │   ├── PlayerInputReader.cs  # Input event reader / state provider
│   │   │   │   └── PlayerMovement.cs     # Rigidbody physics movement controller
│   │   │   └── Sound/                    # Audio subsystem
│   │   │       ├── AudioSettings.cs      # AudioMixer decibel controller & prefs
│   │   │       ├── GetAudio.cs           # UI event audio triggers
│   │   │       ├── Sound.cs              # Sound data definition (name + clip)
│   │   │       └── SoundManager.cs       # Persistent singleton audio controller
│   │   └── Sound/                        # Audio assets & mixers
│   │       ├── Main Mixer.mixer          # Master AudioMixer (Master, SFX, Music, Ambient)
│   │       └── Source/                   # BGM, SFX, and Ambient audio clips
│   ├── LeanTween/                        # Open-source tweening engine
│   ├── Plugins/                          # External libraries (CW LeanPool, LeanCommon)
│   ├── OpalStudio/                       # Editor utility (CustomToolbar)
│   ├── Sortify/                          # Editor hierarchy organizer & custom attributes
│   ├── Settings/                         # URP pipeline assets and renderers
│   └── TextMesh Pro/                     # TextMeshPro fonts, shaders, and resources
├── Packages/
│   └── manifest.json                     # Unity Package Manager manifest
├── ProjectSettings/                      # Project-wide settings (TagManager, EditorBuildSettings, etc.)
└── context/                              # AGENT CONTEXT & DOCUMENTATION REPOSITORY
```

---

## 4. Build Configuration

Configured in `ProjectSettings/EditorBuildSettings.asset`:

| Index | Scene Path | Status in Build |
| :---: | :--- | :---: |
| `0` | `Assets/Project/Scenes/MainMenu.unity` | **Enabled** (Default scene) |
| `1` | `Assets/Project/Scenes/GamePlay.unity` | **Enabled** (Playable level) |
| - | `Assets/Project/Scenes/SampleScene.unity` | Disabled |
