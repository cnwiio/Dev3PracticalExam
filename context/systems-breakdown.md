# Systems Breakdown

This document provides a detailed technical breakdown of each custom subsystem within `Assets/Project/Scripts/`.

---

## 1. Player & Input System

Located in `Assets/Project/Scripts/Player/`.

### 1.1 `PlayerMovement.cs`
- **Purpose:** Applies 3D physics forces to the player character using Unity's `Rigidbody`.
- **Key Fields:**
  - `[SerializeField] private Rigidbody rg`: Reference to the physics body.
  - `private PlayerInputReader inputReader`: Dependency injected via VContainer.
- **Key Methods:**
  - `[Inject] public void Construct(PlayerInputReader playerInputReader)`: Injects the input reader.
  - `Update()`: Calls `HandleHorizontalMovement()`.
  - `HandleHorizontalMovement()`: Translates `inputReader.MovementInput` into a 3D horizontal force:
    ```csharp
    Vector3 movementDirection = new Vector3(inputReader.MovementInput.x, 0, inputReader.MovementInput.y).normalized * 10;
    rg.AddForce(movementDirection, ForceMode.Force);
    ```
- **Agent Note:** Physics additions should ideally run inside `FixedUpdate()` rather than `Update()`.

---

### 1.2 `PlayerInputReader.cs`
- **Purpose:** Bridges Unity New Input System events with game logic, exposing clean properties.
- **Key Properties:**
  - `public Vector2 MovementInput { get; private set; }`: Normalized 2D movement vector (WASD / Left Stick).
- **Key Fields:**
  - `[SerializeField] private InputActionReference MoveAction`: Reference to the `Move` action in `InputSystem_Actions`.
- **Lifecycle:**
  - `OnEnable()`: Subscribes `performed += OnMove` and `canceled += OnStopMove`.
  - `OnDisable()`: Unsubscribes to avoid memory leaks and ghost callbacks.

---

### 1.3 `InputManager.cs`
- **Purpose:** Manages active action maps to avoid input collisions between UI and gameplay.
- **Action Map Modes:**
  ```csharp
  public enum ActionMapType
  {
      Player,
      UI
  }
  ```
- **Key Methods:**
  - `SwitchToPlayerActionMap()`: Disables UI action map and enables Player action map.
  - `SwitchToUIActionMap()`: Disables Player action map and enables UI action map.
  - Property `CurrentActionMaptype`: Triggers appropriate map switch on change.

---

## 2. Audio Subsystem

Located in `Assets/Project/Scripts/Sound/` and `Assets/Project/Sound/`.

### 2.1 `SoundManager.cs`
- **Pattern:** Persistent Singleton (`DontDestroyOnLoad`).
- **Audio Channels:**
  - `AudioSource bgmSource`: For looping background music.
  - `AudioSource sfxSource`: For one-shot sound effects (`PlayOneShot`).
  - `AudioSource[] ambientSources`: Array of channels for concurrent ambient sounds with stereo panning.
- **Sound Clip Libraries:**
  - `Sound[] bgmSounds`
  - `Sound[] sfxSounds`
  - `Sound[] ambientSounds`
- **Public API:**
  - `PlayBGM(string soundName)`: Plays or switches music if not already playing. Default started in `Start()` is `"Level"`.
  - `PlaySFX(string soundName)`: Triggers one-shot sound effect.
  - `PlayAmbient(string soundName, float panValue = 0f)`: Finds an available ambient channel, sets panning, and loops.
  - `StopAmbient(string soundName)`: Stops the specified ambient track.

---

### 2.2 `AudioSettings.cs`
- **Purpose:** Bridges volume settings (linear `0.0f` to `1.0f`) to `AudioMixer` logarithmic decibel scale.
- **Logarithmic Formula:**
  $$\text{Decibels} = \log_{10}(\text{level}) \times 20\text{ dB}$$
- **Exposed Mixer Parameters:**
  - `"MasterVolume"`
  - `"MusicVolume"`
  - `"SFXVolume"`
  - `"AmbientVolume"`
- **Persistence:** Automatically saves and loads all 4 volume levels via `PlayerPrefs` (default `1.0f`).

---

### 2.3 Supporting Audio Types
- **`Sound.cs`:** Serializable struct holding `string name` and `AudioClip clip`.
- **`GetAudio.cs`:** MonoBehaviour helper allowing Unity UI buttons to trigger SFX without direct code access.

---

## 3. UI & Settings Subsystem

Located in `Assets/Project/Scripts/`.

### 3.1 `GameSettingsManager.cs`
- **Purpose:** Controls display resolution, VSync, target frame rate, fullscreen state, and audio levels.
- **Options Supported:**
  - **Resolutions:** Customizable array (HD `1280x720`, HD+ `1600x900`, FHD `1920x1080`, 2K `2560x1440`).
  - **VSync:** `Off` (`QualitySettings.vSyncCount = 0`) vs `On` (`1`).
  - **Target FPS:** `30`, `60`, `120`, `144`, `-1` (Unlimited).
  - **Fullscreen:** `Windowed` vs `Full Screen`.
  - **Volume Levels:** SFX, Master, Music, Ambient (stepped `0` to `10` / `0%` to `100%`).
- **UI Feedback:** Updates TextMeshPro labels in real time.

---

### 3.2 `ClickableText.cs`
- **Purpose:** Interactive text button component enhancing TextMeshPro elements with LeanTween animations and mouse wheel support.
- **Interfaces Implemented:**
  - `IPointerClickHandler`: Fires `onLeftClick` or `onRightClick` UnityEvents + `"Click"` SFX.
  - `IPointerEnterHandler`: Animates scale up (`1.1x`, `0.15s`, `LeanTweenType.easeOutBack`), turns text `Color.yellow`, plays `"Hover"` SFX.
  - `IPointerExitHandler`: Restores scale and initial color.
  - `IScrollHandler`: Detects mouse scroll delta (`eventData.scrollDelta.y`), invoking `onScrollUp` or `onScrollDown` for quick setting adjustments.

---

## 4. Scene Management

Located in `Assets/Project/Scripts/ScenesManager.cs`.

- **Key Methods:**
  - `OpenScene()`: Loads the gameplay level asynchronously via `SceneManager.LoadSceneAsync(1)`.
  - `CloseGame()`: Exits the application via `Application.Quit()`.
