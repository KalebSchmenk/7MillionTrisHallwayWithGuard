using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;


public class playerController : MonoBehaviour{
    public PlayerMovement playerControls;
    Vector2 moveDirection;

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float _sendToMenuIn = 7.5f;

    public float sprintSpeed = 15.0f;
    public float walkSpeed = 10.0f;

    

    Rigidbody rb;

    private InputAction move;
    private InputAction fire;

    private InputAction sprint;

    public float maxStamina = 100;
    private float stamina = 100;
    

    public Transform lookDirection;

    float hozMove;
    float vertMove;

    Vector3 mDirection;

    public bool hasCrown = false;
    private bool _isCaught = false;
    public bool _wonGame = false;

    public bool isSprinting = false;
    public bool stamEmpty = false;

    public GameObject stamTextObject;
    public TextMeshProUGUI stamText;

    private bool stamRecharge;

    public bool isMoving;

    public AudioSource playerSounds;
    public AudioClip noStam;

    private bool soundDelay;

  
    
        
    

    [SerializeField] GameObject _loseUIParent;


    private void Start() {
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;
    }
    void Awake() {
        playerControls = new PlayerMovement();
          
    }
    void Update() {
        

        if(isSprinting == true){
            stamina -= 10 * Time.deltaTime;
 
        }

        if(isSprinting == false){

        }

        
        moveDirection = move.ReadValue<Vector2>();

        mDirection = lookDirection.forward * moveDirection.y + lookDirection.right * moveDirection.x;

        if(moveDirection.y == 0 && moveDirection.x == 0){
            isMoving = false;
        }
        else{
            isMoving = true;
        }

        if(fire.triggered){
           
        }

        if(stamina <= 0){
            stamina = 0;
            stamEmpty = true;
            isSprinting = false;
            moveSpeed = 10;
            StartCoroutine(StamRecharge());
            if(soundDelay == false){
                playerSounds.clip = noStam;
                playerSounds.Play();
                soundDelay = true;
            }
   
        }

        if(stamina >= maxStamina){
            stamina = maxStamina;
        }

        if(stamRecharge == true){
            if(isSprinting == false){
                stamina += 10 * Time.deltaTime;
                    if(stamina == maxStamina){
                        stamEmpty = false;
                    }
                }
        }

        if(stamina >= 1){
            soundDelay = false;
        }


  

        playerControls.Player.Sprint.canceled += notSprinting;
        if(stamEmpty == false){
            playerControls.Player.Sprint.started += Sprinting;
        }
        int roundedStam = Mathf.CeilToInt(stamina);
        stamText.text = "Stamina: " + roundedStam;
        
    }
    private void OnEnable() {
        move = playerControls.Player.Move;
        fire = playerControls.Player.Fire;
        sprint = playerControls.Player.Sprint;

        
        
        move.Enable();
        fire.Enable();
        sprint.Enable();
        

    }

    private void OnDisable() {
        move.Disable();
        fire.Disable();
        sprint.Disable();
       
        
    }

    private void FixedUpdate() {
        
        // If check that denies movement if the player is caught by the guard or game is over
        if (_isCaught || _wonGame)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        
            rb.AddForce(mDirection.normalized * moveSpeed, ForceMode.Force);
        



        
        
    }

    public void CaughtPlayer()
    {
        _isCaught = true;
        PauseMenuController pauseMenu = GetComponent<PauseMenuController>();
        pauseMenu.ResumeGame();
        _loseUIParent.SetActive(true);
        StartCoroutine(AwaitSendToMainMenu());
    }

    private IEnumerator AwaitSendToMainMenu()
    {
        yield return new WaitForSeconds(_sendToMenuIn);

        // RE-IMPLEMENT THIS ON MAIN BUILD!
        SceneManager.LoadScene("MainMenu");
        // DELETE THIS FOR MAIN BUILD!
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Sprinting(InputAction.CallbackContext context){
            stamRecharge = false;
            moveSpeed = 20;
            isSprinting = true;
        
        
    }

    private void notSprinting(InputAction.CallbackContext context){
        moveSpeed = 10;
        isSprinting = false;
        StartCoroutine(StamRecharge());

        
    }

    private IEnumerator StamRecharge(){
        yield return new WaitForSeconds(3.0f);
        
        stamRecharge = true;

    }

}
