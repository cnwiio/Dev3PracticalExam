# Systems Breakdown

This document provides a detailed technical breakdown of each custom subsystem across `Assets/Project/Scripts/`.

---

## 1. Player & Input System

Located in `Assets/Project/Scripts/Player/`.

### 1.1 `PlayerMovement.cs`
- **Role:** Physics-driven character controller applying forces and setting velocities on a `Rigidbody`.
- **Key Features:**
  - **Physics Step:** Runs entirely within `FixedUpdate()`.
  - **Ground Detection:** Uses `Physics.SphereCast` from player bounds/feet downwards (`stats.groundCheckRadius`, `stats.groundCheckDistance`, `stats.groundMask`).
  - **Coyote Time:** Tracks `lastGroundedTime`; allows jumping for a brief window (`stats.coyoteTime`, default `0.1s`) after leaving a platform ledge.
  - **Jump Buffer:** Tracks `lastJumpPressedTime`; registers a jump press made shortly before touching the ground (`stats.jumpBufferTime`, default `0.1s`).
  - **Variable Jump Height:** Calculates upward velocity via $v = \sqrt{2 \cdot g \cdot h}$. If the jump button is released early while ascending, applies `stats.lowJumpGravityMultiplier` (default `2.5x`) via `ApplyExtraGravity()`.
  - **Snappy Fall:** Applies `stats.fallGravityMultiplier` (default `2.0x`) when falling ($v_y < 0$) for responsive platforming feel.
  - **Horizontal Acceleration/Deceleration:** Uses `Vector3.MoveTowards` on the horizontal velocity plane with distinct rates for acceleration, deceleration, and reduced air control (`stats.airControl`).
  - **Camera-Relative Direction:** Rotates movement input according to `cameraTransform.eulerAngles.y`.
  - **Facing Rotation:** Uses `rg.MoveRotation(Quaternion.Slerp(...))` towards movement direction.
- **Public API:**
  - `bool IsGrounded`: Current grounded state.
  - `bool IsRunning`: True when holding sprint while moving.

---

### 1.2 `PlayerMovementStats.cs` & `PlayerMovementData.cs`
- **Role:** ScriptableObject assets storing tunable parameters live in the Unity Inspector without script recompilation.
- **Parameters:**
  - `moveSpeed = 6f`, `runSpeed = 10f`, `acceleration = 40f`, `deceleration = 50f`
  - `rotationSpeed = 12f`
  - `jumpHeight = 2f`, `fallGravityMultiplier = 2f`, `lowJumpGravityMultiplier = 2.5f`
  - `coyoteTime = 0.1f`, `jumpBufferTime = 0.1f`, `airControl = 0.5f`
  - `groundMask = ~0`, `groundCheckDistance = 0.15f`, `groundCheckRadius = 0.25f`

---

### 1.3 `PlayerInputReader.cs`
- **Role:** Bridges Unity New Input System events with game logic.
- **Exposed Properties:**
  - `Vector2 MovementInput`: 2D move vector (WASD / Left Stick).
  - `Vector2 LookInput`: 2D look delta (Mouse / Right Stick).
  - `bool RunHeld`: True while sprint button is held.
  - `bool JumpHeld`: True while jump button is held.
  - `bool IsUIMode`: True while the UI action map is active.
- **Exposed Events:**
  - `event Action JumpPressed`: Invoked upon pressing jump.
- **Key Method:**
  - `SetUIMode(bool uiMode)`: Toggles between `Player` and `UI` action maps. When switching to UI mode, disables the player map and zeroes out all movement/look/jump state.

---

### 1.4 `InputManager.cs`
- **Role:** Action map coordinator and cursor lock manager.
- **Key Methods:**
  - `SwitchToPlayerActionMap()`: Enables player map, locks and hides cursor.
  - `SwitchToUIActionMap()`: Enables UI map, unlocks and reveals cursor.

---

## 2. Third-Person Camera Subsystem

Located in `Assets/Project/Scripts/Camera/`.

### 2.1 `ThirdPersonCameraController.cs`
- **Role:** Drives a Cinemachine 3 `CinemachineCamera` follow target (`CameraTarget`).
- **Key Features:**
  - **Target Follow:** Positions itself at `player.position + settings.pivotOffset` during `LateUpdate()`.
  - **Pitch & Yaw Rotation:** Accumulates look inputs, clamps pitch between `minPitch` and `maxPitch` (default `-30°` to `+70°`), and applies angle smoothing via `Mathf.SmoothDampAngle`.
  - **Live Settings Sync:** Every frame, updates `CinemachineThirdPersonFollow` fields (`CameraDistance`, `ShoulderOffset`, `CameraSide`, `VerticalArmLength`, `Damping`, and `AvoidObstacles` collision filtering).
  - **Exposed Property:** `float Yaw`: Camera horizontal angle for directional movement.

---

### 2.2 `ThirdPersonCameraSettings.cs`
- **Role:** ScriptableObject holding camera framing, look sensitivity, smoothing, and collision settings.
- **Parameters:**
  - `sensitivity = 0.12f`, `invertY = false`, `minPitch = -30f`, `maxPitch = 70f`
  - `pivotOffset = (0, 1.6, 0)`, `cameraDistance = 5f`, `shoulderOffset = (0.5, -0.2, 0)`, `cameraSide = 0.6f`
  - `damping = (0.1, 0.25, 0.3)`, `rotationSmoothTime = 0.03f`
  - `collisionMask`, `cameraRadius = 0.25f`, `dampingIntoCollision = 0f`, `dampingFromCollision = 0.5f`

---

## 3. Stage 1 Gameplay & Minigame Subsystem

Located in `Assets/Project/Scripts/Stage 1/`.

### 3.1 `TimeCounter.cs`
- **Role:** Controls the 30-second digital countdown clock.
- **Key Features:**
  - Starts at `startTime = 30f` (configurable in inspector).
  - Listens to `ItemCollectedMessage` via MessagePipe; adjusts remaining time by `msg.TimeDelta` (+3s boost, -5s penalty).
  - Updates HUD text formatted as `-- {counter} --`.
  - When time reaches zero, raises `event Action TimeExpired` once.

---

### 3.2 `ScoreManager.cs`
- **Role:** In-memory score domain service (VContainer Singleton).
- **Key Features:**
  - Constant: `WinScore = 10000` (threshold required to win).
  - Listens to `ItemCollectedMessage`; updates `Score` property (+500 / -500).
  - Dispatches `event Action<int> OnScoreChanged`.

---

### 3.3 `ScoreCounter.cs`
- **Role:** HUD View component bound to `ScoreManager.OnScoreChanged`. Displays `Score: {score}`.

---

### 3.4 `GameResultController.cs`
- **Role:** Evaluates Stage 1 win/loss rules according to the GDD.
- **Rules Evaluated:**
  - **Win:** Timer expires (`OnTimeExpired`) **AND** `scoreManager.Score > ScoreManager.WinScore` (strictly $> 10,000$).
  - **Lose (Under Score):** Timer expires **AND** `scoreManager.Score <= ScoreManager.WinScore`.
  - **Lose (Pitfall / Void):** `player.transform.position.y < fallThresholdY` (default `-10f`).
- **Outcome Handling:**
  - Sets `Time.timeScale = 0f`.
  - Unlocks mouse cursor (`CursorLockMode.None`, `Cursor.visible = true`).
  - Activates `winPanel` or `gameOverPanel` with final score and reason.
  - Plays `"Win"` or `"Lose"` SFX via `SoundManager`.
- **Public Button Handlers:**
  - `Retry()`: Restores `Time.timeScale = 1f`, reloads current active scene.
  - `MainMenu()`: Restores `Time.timeScale = 1f`, loads `mainMenuSceneIndex` (Build index 0).

---

### 3.5 `PauseMenuController.cs`
- **Role:** In-game pause modal controller.
- **Key Features:**
  - Creates dedicated `InputAction` listening to `<Keyboard>/escape` and `<Gamepad>/start`.
  - Ignores pause triggers if `GameResultController.IsFinished` is true.
  - `Pause()`: Sets `Time.timeScale = 0f`, enables UI input mode, unlocks cursor, shows pause panel.
  - `Resume()`: Restores `Time.timeScale = 1f`, relocks cursor, hides panels.
  - `OpenSettings()` / `CloseSettings()`: Toggles the `Setting Panel` prefab instance within the pause menu.

---

## 4. Items & Collectibles Subsystem

Located in `Assets/Project/Scripts/Stage 1/Items/`.

### 4.1 `Item.cs`
- **Role:** Pickup / obstacle entity with trigger collider.
- **Workflow:**
  - Injected with `IPublisher<ItemCollectedMessage>` via VContainer `RegisterBuildCallback`.
  - On trigger collision with `PlayerMovement`:
    - Constructs `ItemCollectedMessage`.
    - Publishes the message through MessagePipe.
    - Destroys itself (`Destroy(gameObject)`).

### 4.2 `ItemType.cs`
- `TimeBoost`: $+3\text{ seconds}$
- `ScoreBoost`: $+500\text{ points}$
- `TimePenalty`: $-5\text{ seconds}$
- `ScorePenalty`: $-500\text{ points}$

### 4.3 `ItemCollectedMessage.cs`
- Readonly struct carrying `ItemType Type`, `int ScoreDelta`, `float TimeDelta`, and `bool IsPositive`.

### 4.4 `ItemSfxPlayer.cs`
- Implements `IStartable` and `IDisposable`.
- Subscribes to `ItemCollectedMessage`; triggers `SoundManager.Instance.PlaySFX("ItemCollect")` for positive items or `"ItemHit"` for obstacle penalties.

---

## 5. Audio Subsystem

Located in `Assets/Project/Scripts/Sound/`.

### 5.1 `SoundManager.cs`
- **Pattern:** Persistent Singleton (`DontDestroyOnLoad`).
- **Channels:** `bgmSource` (Looping music), `sfxSource` (One-shots), `ambientSources[]` (Panned stereo ambient tracks).
- **Public API:**
  - `PlayBGM(string soundName)`
  - `PlaySFX(string soundName)`
  - `PlayAmbient(string soundName, float panValue = 0f)`
  - `StopAmbient(string soundName)`

### 5.2 `AudioSettings.cs`
- Bridges volume levels ($0.0$ to $1.0$) to `AudioMixer` logarithmic decibels:
  $$\text{Decibels} = \log_{10}(\text{level}) \times 20\text{ dB}$$
- Persists `"MasterVolume"`, `"MusicVolume"`, `"SFXVolume"`, and `"AmbientVolume"` via `PlayerPrefs`.

---

## 6. UI & Scene Management

- **`GameSettingsManager.cs`:** Controls resolution, VSync, target FPS, fullscreen, and audio levels with `LeanTween.cancelAll()` / `reset()` on `Awake()`.
- **`ClickableText.cs`:** Interactive text button with LeanTween scaling, yellow hover highlight, click events, and mouse scroll delta support.
- **`ScenesManager.cs`:** `OpenScene(int sceneIndex)` (asynchronous scene loading by index) and `CloseGame()`.
