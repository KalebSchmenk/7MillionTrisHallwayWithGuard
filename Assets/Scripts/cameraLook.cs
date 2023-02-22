using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraLook : MonoBehaviour
{
    public PlayerMovement playerControls;
    Vector2 lookDirection;
    private InputAction look;
    public float mouseSens = 25.0f;
    public float controllerSens = 125.0f;

    private float lookSens;


    public Transform pLooking;
    private bool controller;

    float camX;
    float camY;

    float lookX;
    float lookY;

    private bool isPaused;

    public GameObject player;


    private void Awake() 
    {
        playerControls = new PlayerMovement();
    }

    private void Start() 
    {
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() 
    {
        PauseMenuController pauseMenu = player.GetComponent<PauseMenuController>();
        
        
         
        if(Gamepad.current != null)
        {
             lookSens = controllerSens / 100;
        }
        else if (Gamepad.current == null)
        {
             lookSens = mouseSens / 100;
        }

        if(pauseMenu.paused == false){
            lookDirection = look.ReadValue<Vector2>();

            lookX = lookDirection.x * lookSens;
            lookY = lookDirection.y * lookSens;
            
            camY += lookX; 
            camX -= lookY;

            camX = Mathf.Clamp(camX, -90, 90);

            pLooking.transform.rotation = Quaternion.Euler(0, camY, 0);
            transform.rotation = Quaternion.Euler(camX, camY, 0);
        }
       
    }

    private void OnEnable() 
    {
        look = playerControls.Player.Look; 
        look.Enable();
    }

    private void OnDisable() 
    {
        look.Disable();
    }
}
