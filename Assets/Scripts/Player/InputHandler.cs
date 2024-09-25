using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerController playerController;
    /*private PlayerInteraction playerInteraction;
    private Inventory inventory;*/
    private PlayerControls playerControls;
    private CombatController combatController;
    [SerializeField]private SceneLoader loader;
    //[SerializeField] private GameObject player_cam;

    void Start()
    {
        LockCursor(true);
    }

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        combatController = GetComponent<CombatController>();
        loader = FindAnyObjectByType<SceneLoader>();
        /*playerInteraction = GetComponent<PlayerInteraction>();
        inventory = GetComponent<Inventory>();*/
        playerControls = new PlayerControls();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();

        playerControls.Player.Move.performed += OnMoves;
        playerControls.Player.Move.canceled += OnMoves;
        playerControls.Player.Look.performed += OnLooks;
        playerControls.Player.Look.canceled += OnLooks;
        playerControls.Player.Jump.performed += ctx => playerController.isJumping = ctx.ReadValueAsButton();
        playerControls.Player.Sprint.performed += ctx => playerController.isSprinting = ctx.ReadValueAsButton();
        playerControls.Player.Sprint.canceled += ctx => playerController.isSprinting = ctx.ReadValueAsButton();
        playerControls.Player.Crouch.performed += ctx => ToggleCrouch(ctx.ReadValueAsButton());
        // playerControls.Player.Interact.performed += ctx => playerInteraction.Interact();
        playerControls.Player.Scroll.performed += OnScroll;
        playerControls.Player.Scroll.canceled += OnScroll;

        // pickupable items
        playerControls.Player.Interact.performed += ctx => playerController.TryPickupItem();
        playerControls.Player.Interact.canceled += ctx => playerController.currentItem.Throw();

        // attack
        playerControls.Player.Attack.performed += ctx => combatController.Attack();

        // quit
        playerControls.Player.Escape.performed += ctx => loader.OnQuit();
    }

    void OnDisable()
    {
        playerControls.Player.Disable();
    }

    void OnMoves(InputAction.CallbackContext context)
    {
        playerController.moveInput = context.ReadValue<Vector2>();
    }
    void OnLooks(InputAction.CallbackContext context)
    {
        playerController.lookInput = context.ReadValue<Vector2>();
    }

    void OnScroll(InputAction.CallbackContext context)
    {
        /*if (context.ReadValue<float>() > 0)
        {
            inventory.NextTool();
        }
        else if (context.ReadValue<float>() < 0)
        {
            inventory.PreviousTool();
        }*/
    }

    void ToggleCrouch(bool isCrouchButtonPressed)
    {
        if (isCrouchButtonPressed)
        {
            playerController.Crouch();
        }
    }

    private void LockCursor(bool lockState)
    {
        if (lockState)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void EnableInput()
    {
        LockCursor(true);
        //player_cam.SetActive(true);
        playerControls.Player.Enable();
    }

    public void DisableInput()
    {
        LockCursor(true);
        //player_cam.SetActive(false);
        playerControls.Player.Disable();
    }
}
