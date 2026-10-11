# Scenes, Prefabs & Asset Structure

This document details the scenes, key prefabs, and asset structure in the **Dev3PracticalExam** project.

---

## 1. Scenes Overview

Configured in `ProjectSettings/EditorBuildSettings.asset`:

### 1.1 `MainMenu.unity` (Build Index: 0)
- **Path:** `Assets/Project/Scenes/MainMenu.unity`
- **Role:** Entry point of the game application.
- **Key Hierarchy Components:**
  - **LifetimeScope:** `MainMenuLifetimeScope` handles DI for the scene.
  - **Canvas & UI:**
    - Main menu buttons (Start Game, Settings, Exit).
    - `ScenesManager`: Attached to button callbacks for `OpenScene()` and `CloseGame()`.
    - `Setting Panel`: Either pre-instantiated or dynamically enabled modal for settings configuration.
  - **Camera & Lighting:** URP Main Camera and directional ambient light.

---

### 1.2 `GamePlay.unity` (Build Index: 1)
- **Path:** `Assets/Project/Scenes/GamePlay.unity`
- **Role:** Core 3D gameplay scene where player controls and physics take place.
- **Key Hierarchy Components:**
  - **LifetimeScope:** `LevelOneLifetimeScope`:
    - Reference field `playerMovement` pointing to the player GameObject.
    - Reference field `playerInputReader` pointing to the player input component.
  - **Player GameObject:**
    - `Rigidbody`: Physics simulation component with frozen/unfrozen constraints.
    - `PlayerMovement`: Script applying physics forces to the `Rigidbody`.
    - `PlayerInputReader`: Subscribed to `InputSystem_Actions.inputactions` move actions.
  - **Environment:** 3D colliders and geometry.
  - **Camera:** Main camera (integrated with Cinemachine virtual camera if configured).

---

### 1.3 `SampleScene.unity` (Unused)
- **Path:** `Assets/Project/Scenes/SampleScene.unity`
- **Role:** Default scene created by Unity project template. Disabled in build settings.

---

## 2. Key Prefabs

Located in `Assets/Project/Prefabs/`.

### 2.1 `Setting Panel.prefab`
- **Purpose:** Full-featured modular settings menu for audio and graphics.
- **Components:**
  - `GameSettingsManager`: Core logic script managing resolution, FPS, VSync, fullscreen, and volume states.
  - `AudioSettings`: Connected to `Main Mixer.mixer` for logarithmic audio adjustments.
  - `ClickableText`: Attached to interactive options (e.g., Resolution, VSync, Sound volume %) supporting mouse click and mouse wheel scrolling.
- **Usage:** Placed inside a Canvas in `MainMenu` or as a pause menu overlay in `GamePlay`.

---

### 2.2 `Sound Manager.prefab`
- **Purpose:** Persistent audio player.
- **Components:**
  - `SoundManager.cs`: Singleton script with `DontDestroyOnLoad`.
  - **Child AudioSources:**
    - `bgmSource`: AudioSource configured for 2D looping music.
    - `sfxSource`: AudioSource configured for 2D one-shot playback.
    - `ambientSources`: Multiple AudioSources for overlapping ambient sound effects.
  - **Configured Audio Clips:** Pre-wired arrays of `Sound` objects referencing files in `Assets/Project/Sound/Source/`.

---

## 3. Audio & Graphic Assets

### 3.1 Audio Assets (`Assets/Project/Sound/`)
- **Mixer:** `Main Mixer.mixer`
  - Exposed parameters: `MasterVolume`, `MusicVolume`, `SFXVolume`, `AmbientVolume`.
- **Audio Clips:**
  - `Ambient.mp3`: Ambient environment track.
  - `Chunky_Monkey.wav` & `loksii-cocktail_umbrella.mp3`: Music tracks.
  - `Click 1.mp3`: UI button click SFX.
  - `Hover 1.mp3`: UI hover SFX.
  - `RockBreak.mp3`: Gameplay action SFX.

### 3.2 Art & Animation (`Assets/Project/Assets/`)
- `Animation/Walking.anim`: Character walk animation cycle.
- `Character/`: 3D character mesh / model assets.
- `2D/`: Sprites, UI icons, borders.
- `Fonts/`: Custom typography for TextMeshPro.
