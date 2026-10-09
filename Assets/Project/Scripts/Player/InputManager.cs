using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header("Input Action Asset Reference")]
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private String PlayerActionMapName;
    [SerializeField] private String UIActionMapName;

    private InputActionMap playerActionMap;
    private InputActionMap UIActionMap;
    
    public enum ActionMapType
    {
        Player,
        UI
    }
    
    private ActionMapType currentActionMaptype;
    public ActionMapType CurrentActionMaptype
    {
        get => currentActionMaptype;
        set
        {
            currentActionMaptype = value;
            switch (value)
            {
                case ActionMapType.Player:
                    SwitchToPlayerActionMap();
                    break;
                case ActionMapType.UI:
                    SwitchToUIActionMap();
                    break;
            }
        }
    }
    
    private void OnEnable()
    {
        playerActionMap = inputActionAsset.FindActionMap(PlayerActionMapName);
        UIActionMap = inputActionAsset.FindActionMap(UIActionMapName);
        
        CurrentActionMaptype = ActionMapType.Player;
    }

    private void OnDisable()
    {
        UIActionMap?.Disable();
        playerActionMap?.Disable();

        inputActionAsset.Disable();
    }
    
    public void SwitchToPlayerActionMap()
    {
        UIActionMap?.Disable();

        playerActionMap?.Enable();
    }

    public void SwitchToUIActionMap()
    {
        playerActionMap?.Disable();

        UIActionMap?.Enable();
    }
}
