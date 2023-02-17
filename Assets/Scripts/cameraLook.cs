using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraLook : MonoBehaviour
{
public PlayerMovement playerControls;
Vector2 lookDirection;
private InputAction look;
public float camSens;

public Transform pLooking;

float camX;
float camY;


private void Awake() {
    playerControls = new PlayerMovement();
}
private void Start() {
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
}

private void Update() {
        lookDirection = look.ReadValue<Vector2>();
        float lookX = lookDirection.x * Time.deltaTime * camSens;
        float lookY = lookDirection.y * Time.deltaTime * camSens;
        camY += lookX; 
        camX -= lookY; 
        camX = Mathf.Clamp(camX, -90f, 90);

        transform.rotation = Quaternion.Euler(camX, camY, 0);
        pLooking.rotation = Quaternion.Euler(0, camY, 0);
}


private void OnEnable() {
        look = playerControls.Player.Look; 
        look.Enable();
}

private void OnDisable() {
    look.Disable();
}

}
