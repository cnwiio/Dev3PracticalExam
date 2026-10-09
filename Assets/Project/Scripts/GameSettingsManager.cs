using System;
using UnityEngine;
using TMPro; // ������Ѻ TextMeshPro
using System.Collections.Generic;

public class GameSettingsManager : MonoBehaviour
{
    [Header("UI Text References")]
    private TextMeshProUGUI resolutionText;
    public TextMeshProUGUI vsyncText;
    public TextMeshProUGUI fpsText;
    public TextMeshProUGUI fullscreenText; // ������÷Ѵ���

    [Header("Audio Text References")]
    public TextMeshProUGUI masterText;
    public TextMeshProUGUI sfxText;
    public TextMeshProUGUI musicText;    // ������÷Ѵ���
    private TextMeshProUGUI ambientText;  // ������÷Ѵ���

    [Header("Systems")]
    public AudioSettings audioSettings; // �ҡ AudioSettings (���� SoundManager ���) ������ͧ���

    [Header("Custom Settings")]
    [Tooltip("��袹Ҵ˹�Ҩͷ���ͧ��������������͡�� (�� X=1920, Y=1080)")]
    // ���ҧ Array Ẻ��˹�����ͧ��� Inspector ���������������
    public Vector2Int[] customResolutions = new Vector2Int[]
    {
        new Vector2Int(1280, 720),   // HD
        new Vector2Int(1600, 900),   // HD+
        new Vector2Int(1920, 1080),  // Full HD
        new Vector2Int(2560, 1440)   // 2K
    };
    private int currentResIndex = 0; // ���������鹷�� Full HD (Index ��� 2)

    private string[] vsyncOptions = { "Off", "On" };
    private int currentVsyncIndex = 0;
    private string[] fullscreenOptions = { "Windowed", "Full Screen" };
    private int currentFullscreenIndex = 1; // ��������������� 1 (Full Screen)

    private int[] fpsOptions = { 30, 60, 120, 144, -1 }; // -1 ��� ���ӡѴ (Unlimited)
    private int currentFpsIndex = 1; // �������� 60 FPS

    // �дѺ���§ 0 �֧ 10 (�Դ�� 0% �֧ 100%)
    private int currentSfxLevel = 10;
    private int currentMasterLevel = 10;
    private int currentMusicLevel = 10;   // ������÷Ѵ���
    private int currentAmbientLevel = 10; // ������÷Ѵ���

    private void Awake()
    {
        LoadSettings();
    }

    private void Start()
    {
        if (audioSettings == null) audioSettings = FindAnyObjectByType<AudioSettings>();

        FilterSupportedResolutions();
        UpdateUITexts();

        // ������÷Ѵ��� ���ͺѧ�Ѻ�������Ѻ�дѺ���§��� UI �ѹ�յ͹�������
        ApplyVolumeRealtime();
    }

    // ==========================================
    // �ѧ��ѹ�ѻവ���˹ѧ���
    // ==========================================
    private void UpdateUITexts()
    {
        //resolutionText.text = customResolutions[currentResIndex].x + " x " + customResolutions[currentResIndex].y;
        vsyncText.text = vsyncOptions[currentVsyncIndex];
        fpsText.text = fpsOptions[currentFpsIndex] == -1 ? "Unlimited" : fpsOptions[currentFpsIndex].ToString();
        fullscreenText.text = fullscreenOptions[currentFullscreenIndex];
        // �ѻവ����Ţ % �ͧ���§��� 4 ��Ǵ
        masterText.text = (currentMasterLevel * 10) + "%";
        sfxText.text = (currentSfxLevel * 10) + "%";
        musicText.text = (currentMusicLevel * 10) + "%";
        //ambientText.text = (currentAmbientLevel * 10) + "%";

    }

    // ==========================================
    // �ѧ��ѹ��Ѻ��� Resolution
    // ==========================================
    public void NextResolution() { currentResIndex = (currentResIndex + 1) % customResolutions.Length; UpdateUITexts(); }
    public void PrevResolution() { currentResIndex = (currentResIndex - 1 + customResolutions.Length) % customResolutions.Length; UpdateUITexts(); }

    // ==========================================
    // �ѧ��ѹ��Ѻ��� VSync
    // ==========================================
    public void NextVsync() { currentVsyncIndex = (currentVsyncIndex + 1) % vsyncOptions.Length; UpdateUITexts(); }
    public void PrevVsync() { currentVsyncIndex = (currentVsyncIndex - 1 + vsyncOptions.Length) % vsyncOptions.Length; UpdateUITexts(); }

    // ==========================================
    // �ѧ��ѹ��Ѻ��� FPS
    // ==========================================
    public void NextFPS() { currentFpsIndex = (currentFpsIndex + 1) % fpsOptions.Length; UpdateUITexts(); }
    public void PrevFPS() { currentFpsIndex = (currentFpsIndex - 1 + fpsOptions.Length) % fpsOptions.Length; UpdateUITexts(); }

    // ==========================================
    // �ѧ��ѹ��Ѻ����˹�Ҩ�
    // ==========================================
    public void NextFullscreen() { currentFullscreenIndex = (currentFullscreenIndex + 1) % fullscreenOptions.Length; UpdateUITexts(); }
    public void PrevFullscreen() { currentFullscreenIndex = (currentFullscreenIndex - 1 + fullscreenOptions.Length) % fullscreenOptions.Length; UpdateUITexts(); }

    // ==========================================
    // ��Ǵ��Ѻ���§ (�ѻവ���ǹ�ٻ 0 <-> 100%)
    // ==========================================
    public void NextMaster() { currentMasterLevel = (currentMasterLevel + 1) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }
    public void PrevMaster() { currentMasterLevel = (currentMasterLevel - 1 + 11) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }

    public void NextSFX() { currentSfxLevel = (currentSfxLevel + 1) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }
    public void PrevSFX() { currentSfxLevel = (currentSfxLevel - 1 + 11) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }

    public void NextMusic() { currentMusicLevel = (currentMusicLevel + 1) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }
    public void PrevMusic() { currentMusicLevel = (currentMusicLevel - 1 + 11) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }

    public void NextAmbient() { currentAmbientLevel = (currentAmbientLevel + 1) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }
    public void PrevAmbient() { currentAmbientLevel = (currentAmbientLevel - 1 + 11) % 11; UpdateUITexts(); ApplyVolumeRealtime(); }

    private void ApplyVolumeRealtime()
    {
        if (audioSettings != null)
        {
            float masterVol = currentMasterLevel == 0 ? 0.0001f : (float)currentMasterLevel / 10f;
            float sfxVol = currentSfxLevel == 0 ? 0.0001f : (float)currentSfxLevel / 10f;
            float musicVol = currentMusicLevel == 0 ? 0.0001f : (float)currentMusicLevel / 10f;
            float ambientVol = currentAmbientLevel == 0 ? 0.0001f : (float)currentAmbientLevel / 10f;

            audioSettings.SetMasterVolume(masterVol);
            audioSettings.SetSFXVolume(sfxVol);
            audioSettings.SetMusicVolume(musicVol);
            audioSettings.SetAmbientVolume(ambientVol);
        }
    }

    // ==========================================
    // ���� Apply Change (�׹�ѹ��õ�駤�ҡ�ҿԡ��кѹ�֡)
    // ==========================================
    public void ApplyChanges()
    {
        // 1. �ŧ��� Index �� True/False (1 ��� Full Screen, 0 ��� Windowed)
        bool isFullscreen = (currentFullscreenIndex == 1);

        // 2. ��䢺�÷Ѵ SetResolution ��������� isFullscreen ���������ҧ���
        Vector2Int res = customResolutions[currentResIndex];
        Screen.SetResolution(res.x, res.y, isFullscreen);
        QualitySettings.vSyncCount = currentVsyncIndex;
        Application.targetFrameRate = fpsOptions[currentFpsIndex];

        // ����¹���� Key �� ...LevelUI ���������骹�Ѻ AudioSettings
        PlayerPrefs.SetInt("ResIndex", currentResIndex);
        PlayerPrefs.SetInt("VsyncIndex", currentVsyncIndex);
        PlayerPrefs.SetInt("FpsIndex", currentFpsIndex);
        PlayerPrefs.SetInt("FullscreenIndex", currentFullscreenIndex);
        PlayerPrefs.SetInt("MasterLevelUI", currentMasterLevel);
        PlayerPrefs.SetInt("SFXLevelUI", currentSfxLevel);
        PlayerPrefs.SetInt("MusicLevelUI", currentMusicLevel);
        PlayerPrefs.SetInt("AmbientLevelUI", currentAmbientLevel);
        PlayerPrefs.Save();

        //Debug.Log("Settings Applied and Saved!");
    }

    private void LoadSettings()
    {
        int savedResIndex = PlayerPrefs.GetInt("ResIndex", currentResIndex);
        currentResIndex = Mathf.Clamp(savedResIndex, 0, customResolutions.Length - 1);
        currentVsyncIndex = PlayerPrefs.GetInt("VsyncIndex", 0);
        currentFpsIndex = PlayerPrefs.GetInt("FpsIndex", 1);
        currentFullscreenIndex = PlayerPrefs.GetInt("FullscreenIndex", 1);

        // ��Ŵ������������� Key ����
        currentMasterLevel = PlayerPrefs.GetInt("MasterLevelUI", 4);
        currentSfxLevel = PlayerPrefs.GetInt("SFXLevelUI", 4);
        currentMusicLevel = PlayerPrefs.GetInt("MusicLevelUI", 4);
        currentAmbientLevel = PlayerPrefs.GetInt("AmbientLevelUI", 4);
    }

    private void FilterSupportedResolutions()
    {
        // �Ҥ�� Resolution �٧�ش���˹�Ҩͧ͢�������ͧ�Ѻ�� (�ѡ���������ش���¢ͧ Array)
        Resolution maxMonitorRes = Screen.resolutions[Screen.resolutions.Length - 1];

        List<Vector2Int> validResolutions = new List<Vector2Int>();

        // ǹ�ٻ�� customResolutions �����ҵ�駤������ Inspector
        foreach (Vector2Int res in customResolutions)
        {
            // ��Ҥ������ҧ��Ф����٧ ���¡���������ҡѺ �ͧ͢������ ��������
            if (res.x <= maxMonitorRes.width && res.y <= maxMonitorRes.height)
            {
                validResolutions.Add(res);
            }
        }

        // ���͡óթء�Թ: �������ը��˹��ҹ���͹���� ����ִ��Ҵ���٧�ش�ͧ����������
        if (validResolutions.Count == 0)
        {
            validResolutions.Add(new Vector2Int(maxMonitorRes.width, maxMonitorRes.height));
        }

        // �Ӥ�ҷ���ҹ��äѴ��ͧ���� 价Ѻ Array ������
        customResolutions = validResolutions.ToArray();
    }
}