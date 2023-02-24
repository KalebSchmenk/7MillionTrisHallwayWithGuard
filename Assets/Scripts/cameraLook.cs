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

    private bool isSprinting;
    private bool stamEmpty;

    private float sprintFOV = 70.0f;
    private float walkFOV = 60.0f;
    private float baseFOV = 60.0f;

    public bool moving;

    private bool isMoving;

    public Animator bobAnim;
    private void Update() {
        playerController playerController = player.GetComponent<playerController>();

        isSprinting = playerController.isSprinting;
        stamEmpty = playerController.stamEmpty;
        isMoving = playerController.isMoving;
        
        if(walkFOV > sprintFOV){
            walkFOV = sprintFOV;
        }
        if(walkFOV < baseFOV){
            walkFOV = baseFOV;
        }

  


        if(isSprinting == true){
            
                walkFOV += 10 * Time.deltaTime;
                Camera.main.fieldOfView = walkFOV;
                moving = true;
                bobAnim.SetTrigger("run");
                bobAnim.ResetTrigger("idle");
                bobAnim.ResetTrigger("walk");
            

        }

        if(isSprinting == false && isMoving == true){
            bobAnim.SetTrigger("walk");
            bobAnim.ResetTrigger("idle");
            bobAnim.ResetTrigger("run");
        }

        if(isMoving == false){
            bobAnim.SetTrigger("idle");
            bobAnim.ResetTrigger("walk");
            bobAnim.ResetTrigger("run");
        }

        if(isSprinting == false){
            walkFOV -= 15 *Time.deltaTime;
            Camera.main.fieldOfView = walkFOV;
            moving = false;
            


        }

    }

    private void Awake() 
    {
        
        playerControls = new PlayerMovement();

    }

    private void Start() 
    {
        bobAnim = GetComponent<Animator>();
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() 
    {
        PauseMenuController pauseMenu = player.GetComponent<PauseMenuController>();
  
        
        if(Gamepad.current != null)
        {
            lookSens = controllerSens / 100 * LookSensManager._sliderValue;
        }
        else if (Gamepad.current == null)
        {
            lookSens = mouseSens / 100 * LookSensManager._sliderValue;
        }

        if(pauseMenu._paused == false){
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
