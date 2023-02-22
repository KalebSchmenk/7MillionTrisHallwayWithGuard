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


    private void Awake() 
    {
        playerControls = new PlayerMovement();
    }

    private void Start() 
    {
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() 
    {
        if(Gamepad.current != null)
        {
             lookSens = controllerSens;
        }

        if(Gamepad.current == null)
        {
             lookSens = mouseSens;
        }

        float lookX = 0;
        float lookY = 0;

        lookDirection = look.ReadValue<Vector2>();

        lookX = lookDirection.x * lookSens * Time.deltaTime;
        lookY = lookDirection.y * lookSens * Time.deltaTime;
        
        camY += lookX; 
        camX -= lookY;

        camX = Mathf.Clamp(camX, -90, 90);

        pLooking.transform.rotation = Quaternion.Euler(0, camY, 0);
        transform.rotation = Quaternion.Euler(camX, 0, 0);
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
