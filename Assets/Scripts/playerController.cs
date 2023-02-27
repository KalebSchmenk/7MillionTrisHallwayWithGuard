using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


public class playerController : MonoBehaviour{
    public PlayerMovement playerControls;
    Vector2 moveDirection;

    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float _sendToMenuIn = 7.5f;

    public float sprintSpeed = 20.0f;
    public float walkSpeed = 15.0f;

    

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
    public bool _isCaught = false;
    public bool _wonGame = false;

    public bool isSprinting = false;
    public bool stamEmpty = false;


    private bool stamRecharge;

    public bool isMoving;

    public AudioSource playerSounds;
    public AudioClip noStam;

    private bool soundDelay;

    public Image stamMask;
    private float normSize;

    public GameObject enemy;

    private bool isChasing;

    public GameObject walkObject;
    public GameObject sprintObject;
    public GameObject backgroundmusicObject;
    public GameObject chaseMusicObject;
    public GameObject menuSoundsObject;
    public GameObject miscSoundsObject;

    public AudioSource loseSoundSource;
    public AudioClip loseSound;

    private bool loseSoundPlayed = false;

    public GameObject heldCrown;

    private Animator _animator;
    private Rigidbody _rb;
    
    [SerializeField] GameObject _loseUIParent;


    private void Start() {
        normSize = stamMask.rectTransform.rect.width;
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;
        Time.timeScale = 1;

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();

        stamina = maxStamina;
        
    }
    void Awake() {
        playerControls = new PlayerMovement();
          
    }
    void Update() {

        if (_rb.velocity.magnitude > 0.01f) 
        {
            _animator.Play("Walk");
        }
        else
        {
            _animator.Play("Idle");
        }

        if(hasCrown == true){
            heldCrown.SetActive(true);
        }
        StamVisualValue(stamina/(float)maxStamina);

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
            moveSpeed = walkSpeed;
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
        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        int roundedStam = Mathf.CeilToInt(stamina);
       

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
            
        walkObject.SetActive(false);
        sprintObject.SetActive(false);
        backgroundmusicObject.SetActive(false);
        chaseMusicObject.SetActive(false);
        menuSoundsObject.SetActive(false);
        miscSoundsObject.SetActive(false);
        playerSounds.mute = true;
        if(loseSoundPlayed == false){
            loseSoundSource.PlayOneShot(loseSound);
            loseSoundPlayed = true;
        }
        StartCoroutine(AwaitSendToMainMenu());
    }

    private IEnumerator AwaitSendToMainMenu()
    {
        yield return new WaitForSeconds(_sendToMenuIn);

        SceneManager.LoadScene("MainMenu");
    }

    private void Sprinting(InputAction.CallbackContext context){
            stamRecharge = false;
            moveSpeed = sprintSpeed;
            isSprinting = true;
        
        
    }

    private void notSprinting(InputAction.CallbackContext context){
        moveSpeed = walkSpeed;
        isSprinting = false;
        StartCoroutine(StamRecharge());

        
    }

    private IEnumerator StamRecharge(){
        yield return new WaitForSecondsRealtime(3.0f);
        
        stamRecharge = true;

    }

    public void StamVisualValue(float amount){
        stamMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, normSize * amount);
    }

}
