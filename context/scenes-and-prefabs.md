# Scenes, Prefabs & Asset Structure

This document details the scenes, modular prefabs, and asset layout in the **Dev3PracticalExam** project.

---

## 1. Scenes Breakdown

### 1.1 `MainMenu.unity` (Build Index: 0)
- **Path:** `Assets/Project/Scenes/MainMenu.unity`
- **Role:** Main application entry point and navigation hub.
- **Key Components:**
  - **LifetimeScope:** `MainMenuLifetimeScope` handles DI for the scene.
  - **Canvas & UI:**
    - Navigation buttons: Start (opens Stage Selection), Options (opens `Setting Panel`), Credits.
    - Scene Identity: UI text displaying the examination header per GDD Section 4.1.
    - `ScenesManager`: Invokes `OpenScene(int sceneIndex)` and `CloseGame()`.
  - **Audio:** `SoundManager.prefab` ensures continuous BGM starts here.

---

### 1.2 `GamePlay.unity` (Build Index: 1 — Stage 1: Capsule Player)
- **Path:** `Assets/Project/Scenes/GamePlay.unity`
- **Role:** Core 3D Time Attack minigame featuring the physics-driven Capsule Player.
- **Key Hierarchy Components:**
  - **`LevelOneLifetimeScope`:** Orchestrates scene dependencies, MessagePipe broker, and build callbacks.
  - **Player (`Assets/Project/Prefabs/Player/Player.prefab`):**
    - `Rigidbody` with frozen rotation constraints and `CapsuleCollider`.
    - `PlayerMovement`: Consumes `PlayerMovementStats` ScriptableObject, references Main Camera for camera-relative movement.
    - `PlayerInputReader`: Configured with Move, Look, Run, and Jump action references.
  - **Camera System:**
    - `CameraTarget` (`Assets/Project/Prefabs/Player/CameraTarget.prefab`): Unparented scene root object running `ThirdPersonCameraController` with `ThirdPersonCameraSettings`.
    - `CinemachineCamera` (`Assets/Project/Prefabs/Player/CinemachineCamera.prefab`): Cinemachine 3 follow camera tracking `CameraTarget`.
    - `Main Camera`: Holds `CinemachineBrain` component.
  - **Level Geometry (ProBuilder):**
    - Wide platforms, narrow catwalk beams, and unbordered drop-offs leading into the void ($Y < -10$).
  - **Item Placements:**
    - Distributed instances of the 4 item prefabs (`Item Add Score`, `Item Add Time`, `Item Minus Score`, `Item Minus Time`). Total score sum exceeds 10,000 points.
  - **Canvas & HUD (`Assets/Project/Prefabs/UI/`):**
    - `HUD.prefab`: Top-left digital countdown timer (`TimeCounter`) and score counter (`ScoreCounter`).
    - `PauseMenuController.prefab`: Pauses time (`Time.timeScale = 0`), unlocks cursor, and hosts Retry / Settings / Main Menu buttons.
    - `GameResultController.prefab`: Evaluates win/loss conditions, showing `WinPanel` or `GameOverPanel`.
    - `Setting Panel.prefab`: Nested within the Canvas, accessible via pause menu or options.

---

### 1.3 `Stage2.unity` (Stage 2: Humanoid Player)
- **Path:** `Assets/Project/Scenes/Stage2.unity`
- **Role:** Second examination minigame scene (in development per GDD Section 3.2).
- **Design Target:**
  - Humanoid character controller using Unity Starter Assets.
  - ProBuilder arena with minimum surface area $\ge 50\text{ m}^2$.
  - Proximity/explicit interaction system (e.g. `[E]` key) instead of trigger collision pickups.

---

### 1.4 `SampleScene.unity` (Disabled)
- **Path:** `Assets/Project/Scenes/SampleScene.unity`
- **Role:** Unity template scene, excluded from active build order.

---

## 2. Prefabs Breakdown

All modular game prefabs reside in `Assets/Project/Prefabs/`:

### 2.1 Collectibles & Obstacles (`Prefabs/Item/`)

| Prefab Name | Script | `ItemType` | Impact |
| :--- | :--- | :--- | :--- |
| **`Item Add Score.prefab`** | `Item.cs` | `ScoreBoost` | $+500\text{ pts}$, positive pickup SFX |
| **`Item Add Time.prefab`** | `Item.cs` | `TimeBoost` | $+3\text{ seconds}$, positive pickup SFX |
| **`Item Minus Score.prefab`** | `Item.cs` | `ScorePenalty` | $-500\text{ pts}$, obstacle collision SFX |
| **`Item Minus Time.prefab`** | `Item.cs` | `TimePenalty` | $-5\text{ seconds}$, obstacle collision SFX |

*All items require a Collider with `Is Trigger = true`.*

---

### 2.2 Player & Camera (`Prefabs/Player/`)

| Prefab Name | Attached Components | Role |
| :--- | :--- | :--- |
| **`Player.prefab`** | `Rigidbody`, `CapsuleCollider`, `PlayerMovement` | Physics-driven platformer character. |
| **`PlayerInputReader.prefab`** | `PlayerInputReader` | Unity New Input System reader component. |
| **`CameraTarget.prefab`** | `ThirdPersonCameraController` | Independent follow target providing yaw/pitch for Cinemachine. |
| **`CinemachineCamera.prefab`** | `CinemachineCamera`, `CinemachineThirdPersonFollow` | Procedural orbital third-person camera. |

---

### 2.3 User Interface (`Prefabs/UI/`)

| Prefab Name | Scripts Attached | Role |
| :--- | :--- | :--- |
| **`HUD.prefab`** | `TimeCounter`, `ScoreCounter` | In-game digital countdown clock and score HUD. |
| **`PauseMenuController.prefab`** | `PauseMenuController` | Freezes game, unlocks cursor, routes to retry or settings. |
| **`GameResultController.prefab`** | `GameResultController` | Displays Victory / Game Over panels and handles replay. |
| **`Setting Panel.prefab`** | `GameSettingsManager`, `AudioSettings`, `ClickableText` | Resolution, VSync, FPS, fullscreen, and volume options. |

---

### 2.4 Audio (`Prefabs/`)

| Prefab Name | Pattern | Role |
| :--- | :--- | :--- |
| **`Sound Manager.prefab`** | Singleton (`DontDestroyOnLoad`) | Houses BGM source, SFX source, ambient sources, and pre-wired sound libraries. |

---

## 3. Audio & Asset Library

Located in `Assets/Project/Sound/` and `Assets/Project/Assets/`:

- **Mixer:** `Main Mixer.mixer` (Groups: `Master`, `Music`, `SFX`, `Ambient`).
- **Sound Clips (`Source/`):**
  - `Ambient.mp3`: Ambient background layer.
  - `Chunky_Monkey.wav` & `loksii-cocktail_umbrella.mp3`: Background music tracks.
  - `Click 1.mp3`: UI button click.
  - `Hover 1.mp3`: UI hover tone.
  - `RockBreak.mp3`: Item hit / obstacle penalty SFX.
- **Visual Assets:**
  - `Animation/Walking.anim`: Character walk animation cycle.
  - `Fonts/`: TextMeshPro font assets.
  - `2D/`: UI sprites, icons, and frames.
