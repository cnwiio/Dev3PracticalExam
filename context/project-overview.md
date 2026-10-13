# Project Overview & Tech Stack

This document details the environment, technology stack, directory organization, and package dependencies for the **Dev3PracticalExam** project.

---

## 1. Engine & Environment

- **Unity Editor Version:** `6000.3.18f1` (Unity 6 LTS)
- **Target Platform:** Desktop / PC (Windows Standalone)
- **Render Pipeline:** Universal Render Pipeline (URP `17.3.0`)
- **Scripting Backend / .NET:** .NET Standard 2.1 / C# 9.0+

---

## 2. Key Package Dependencies

Configured in `Packages/manifest.json`:

| Package | Version | Source | Purpose |
| :--- | :--- | :--- | :--- |
| `jp.hadashikick.vcontainer` | `1.19.0` | Git | High-performance, lightweight Dependency Injection framework for Unity. |
| `com.cysharp.messagepipe` | Latest | Git | High-speed in-memory publish/subscribe messaging pipeline. |
| `com.cysharp.messagepipe.vcontainer` | Latest | Git | MessagePipe container builder extension for VContainer. |
| `com.cysharp.unitask` | Latest | Git | Zero-allocation async/await integration for Unity. |
| `com.unity.inputsystem` | `1.19.0` | UPM | Unity New Input System (action maps, callbacks, gamepad/keyboard). |
| `com.unity.cinemachine` | `3.1.7` | UPM | Third-person follow camera, procedural orbital tracking, and damping. |
| `com.unity.probuilder` | `6.0.9` | UPM | In-editor 3D level geometry modeling, platforms, catwalks, and arenas. |
| `com.unity.render-pipelines.universal` | `17.3.0` | UPM | Universal Render Pipeline shaders, render features, and lighting. |
| `com.unity.ai.navigation` | `2.0.13` | UPM | NavMesh surfaces and AI pathfinding. |
| `com.unity.ugui` | `2.0.0` | UPM | TextMeshPro, Canvas UI, and EventSystem. |
| `com.unity.timeline` | `1.8.12` | UPM | Cutscenes, sequencing, and multi-track animation. |

---

## 3. Directory Structure

The repository maintains a clean modular separation between gameplay code, editor utilities, manual guides, and context documents:

```
Dev3PracticalExam/
├── Assets/
│   ├── InputSystem_Actions.inputactions   # Root InputActionAsset (Player, UI maps)
│   ├── Project/                          # PRIMARY GAME CODE & ASSETS
│   │   ├── Assets/                       # Art, 2D sprites, animations, fonts
│   │   │   ├── 2D/
│   │   │   ├── Animation/
│   │   │   ├── Character/
│   │   │   └── Fonts/
│   │   ├── Prefabs/                      # Organized GameObject prefabs
│   │   │   ├── Item/                     # Collectibles & Obstacles
│   │   │   │   ├── Item Add Score.prefab
│   │   │   │   ├── Item Add Time.prefab
│   │   │   │   ├── Item Minus Score.prefab
│   │   │   │   └── Item Minus Time.prefab
│   │   │   ├── Player/                   # Player, Camera Target, Cinemachine
│   │   │   │   ├── CameraTarget.prefab
│   │   │   │   ├── CinemachineCamera.prefab
│   │   │   │   ├── Player.prefab
│   │   │   │   └── PlayerInputReader.prefab
│   │   │   ├── UI/                       # UI Controllers & HUD
│   │   │   │   ├── GameResultController.prefab
│   │   │   │   ├── HUD.prefab
│   │   │   │   ├── PauseMenuController.prefab
│   │   │   │   └── Setting Panel.prefab
│   │   │   └── Sound Manager.prefab      # Persistent audio manager prefab
│   │   ├── Scenes/                       # Active game scenes
│   │   │   ├── MainMenu.unity            # Start menu scene (Build index 0)
│   │   │   ├── GamePlay.unity            # Stage 1: Capsule Player (Build index 1)
│   │   │   ├── Stage2.unity              # Stage 2: Humanoid Player (In development)
│   │   │   └── SampleScene.unity         # Default template scene (Disabled)
│   │   ├── Scripts/                      # Custom C# gameplay and architecture code
│   │   │   ├── ClickableText.cs          # Interactive text UI component with LeanTween
│   │   │   ├── GameSettingsManager.cs    # Graphics/audio options & PlayerPrefs manager
│   │   │   ├── LevelOneLifetimeScope.cs  # VContainer + MessagePipe scope for GamePlay
│   │   │   ├── MainMenuLifetimeScope.cs  # VContainer scope for MainMenu scene
│   │   │   ├── ScenesManager.cs          # Scene loading (by index) & app quit
│   │   │   ├── Camera/                   # Third-Person Camera Subsystem
│   │   │   │   ├── ThirdPersonCameraController.cs  # Cinemachine 3 target driver
│   │   │   │   └── ThirdPersonCameraSettings.cs    # ScriptableObject camera stats
│   │   │   ├── Player/                   # Player Subsystem
│   │   │   │   ├── InputManager.cs       # ActionMap switcher & cursor state
│   │   │   │   ├── PlayerInputReader.cs  # Move, Look, Run, Jump, UI mode reader
│   │   │   │   ├── PlayerMovement.cs     # Rigidbody physics movement & jump controller
│   │   │   │   ├── PlayerMovementData.cs # ScriptableObject movement data
│   │   │   │   └── PlayerMovementStats.cs# ScriptableObject live movement & jump stats
│   │   │   ├── Sound/                    # Audio Subsystem
│   │   │   │   ├── AudioSettings.cs      # AudioMixer decibel controller & prefs
│   │   │   │   ├── GetAudio.cs           # UI event audio triggers
│   │   │   │   ├── Sound.cs              # Sound data definition (name + clip)
│   │   │   │   └── SoundManager.cs       # Persistent singleton audio controller
│   │   │   └── Stage 1/                  # Stage 1 Minigame Subsystem
│   │   │       ├── GameResultController.cs  # Win/Lose rule evaluator & panels
│   │   │       ├── PauseMenuController.cs   # Pause, resume, retry, settings
│   │   │       ├── ScoreCounter.cs          # HUD score view
│   │   │       ├── ScoreManager.cs          # Score domain state & WinScore threshold
│   │   │       ├── TimeCounter.cs           # 30s countdown timer & TimeExpired event
│   │   │       └── Items/                   # Collectibles & Obstacles logic
│   │   │           ├── Item.cs              # Trigger pickup publisher
│   │   │           ├── ItemCollectedMessage.cs # MessagePipe event payload
│   │   │           ├── ItemSfxPlayer.cs     # Pickup audio subscriber (IStartable)
│   │   │           └── ItemType.cs          # Enum (4 item kinds)
│   │   └── Sound/                        # Audio assets & mixers
│   │       ├── Main Mixer.mixer          # Master AudioMixer (Master, SFX, Music, Ambient)
│   │       └── Source/                   # BGM, SFX, and Ambient audio clips
│   ├── LeanTween/                        # Open-source tweening engine
│   ├── Plugins/                          # External libraries (CW LeanPool, LeanCommon)
│   ├── OpalStudio/                       # Editor utility (CustomToolbar)
│   ├── Sortify/                          # Editor hierarchy organizer & custom attributes
│   ├── Settings/                         # URP pipeline assets and renderers
│   └── TextMesh Pro/                     # TextMeshPro fonts, shaders, and resources
├── Manuals/                              # EDITOR SETUP GUIDES
│   ├── Pause_Menu_Manual.txt             # Step-by-step pause panel setup
│   ├── Run_Jump_System_Manual.txt        # Rigidbody run/jump tuning manual
│   ├── Third-Person_Camera_Manual.txt    # Cinemachine 3 camera setup manual
│   └── Win_Lose_System_Manual.txt        # Stage 1 win/lose controller setup manual
├── Packages/
│   └── manifest.json                     # Unity Package Manager manifest
├── ProjectSettings/                      # Project-wide editor & build configuration
└── context/                              # AGENT CONTEXT REPOSITORY
    ├── GDD.md                            # Complete Game Design Document & Rubric
    ├── README.md                         # Master index & quick guide
    ├── project-overview.md               # Tech stack & directory structure
    ├── architecture.md                   # VContainer + MessagePipe architecture
    ├── systems-breakdown.md              # Deep dive into all gameplay systems
    ├── scenes-and-prefabs.md             # Scenes, prefabs, and asset layout
    └── conventions-and-guidelines.md     # Coding rules & agent checklist
```

---

## 4. Build Configuration

Configured in `ProjectSettings/EditorBuildSettings.asset`:

| Index | Scene Path | Status in Build | Role |
| :---: | :--- | :---: | :--- |
| `0` | `Assets/Project/Scenes/MainMenu.unity` | **Enabled** | Main menu, Options modal, Stage select |
| `1` | `Assets/Project/Scenes/GamePlay.unity` | **Enabled** | Stage 1: Capsule Player Time Attack minigame |
| - | `Assets/Project/Scenes/Stage2.unity` | Pending add | Stage 2: Humanoid Player minigame |
| - | `Assets/Project/Scenes/SampleScene.unity` | Disabled | Template scratch scene |
