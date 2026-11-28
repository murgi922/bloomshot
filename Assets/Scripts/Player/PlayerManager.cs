using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private InputAction interactAction;
    private InputAction prevAction;
    public enum CurrentEquipment
    {
        None,
        WateringCan,
        Hoe,
        Seed
    }
    [SerializeField] private CurrentEquipment currentEquipment;
    public CurrentEquipment Equipment => currentEquipment;
    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        if (interactAction != null) interactAction.Enable();
        else Debug.LogError("Interact action could not be found in player manager!");

        prevAction = InputSystem.actions.FindAction("Previous");
        if (prevAction != null) prevAction.Enable();
        else Debug.LogError("Previous action could not be found in player manager!");
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            TileManager.Instance.InteractActionPressed();
        }
        if (prevAction.WasPressedThisFrame())
        {
            EquipmentManager.Instance.CycleEquipment();
        }
    }
}
