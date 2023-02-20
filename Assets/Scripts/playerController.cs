using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour{
    public PlayerMovement playerControls;
    Vector2 moveDirection;

    [SerializeField] float playerSpeed = 10.0f;
    [SerializeField] float _sendToMenuIn = 7.5f;

    Rigidbody rb;

    private InputAction move;

    public Transform lookDirection;

    float hozMove;
    float vertMove;

    Vector3 mDirection;

    public bool hasCrown = false;
    private bool _isCaught = false;
    public bool _wonGame = false;
    [SerializeField] GameObject loseUIParent;


    private void Start() {
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;
    }
    void Awake() {
        playerControls = new PlayerMovement();
          
    }
    void Update() {
        moveDirection = move.ReadValue<Vector2>();
 
        if(Input.GetKeyDown(KeyCode.Escape)){
            // DELETE ON BUILD
            Application.Quit();
        }

        mDirection = lookDirection.forward * moveDirection.y + lookDirection.right * moveDirection.x;
    }
    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();

    }

    private void OnDisable() {
        move.Disable();
        
    }

    private void FixedUpdate() {

        // If check that denies movement if the player is caught by the guard
        if (_isCaught || _wonGame)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        rb.AddForce(mDirection.normalized * playerSpeed, ForceMode.Force);

        
    }

    public void CaughtPlayer()
    {
        _isCaught = true;
        loseUIParent.SetActive(true);
        StartCoroutine(AwaitSendToMainMenu());
    }

    private IEnumerator AwaitSendToMainMenu()
    {
        yield return new WaitForSeconds(_sendToMenuIn);

        // RE-IMPLEMENT THIS ON MAIN BUILD!
        //SceneManager.LoadScene("MainMenu");
        // DELETE THIS FOR MAIN BUILD!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
