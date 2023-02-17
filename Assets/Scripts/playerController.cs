using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour{
    public PlayerMovement playerControls;
    Vector2 moveDirection;

    [SerializeField] float playerSpeed = 10.0f;

    Rigidbody rb;

    private InputAction move;

    public Transform lookDirection;

    float hozMove;
    float vertMove;

    Vector3 mDirection;

    public bool hasCrown = false;
    private bool _isCaught = false;
    private GameObject _guard;
    private Vector3 _rotateTowards;

    private void Start() {
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;
    }
    void Awake() {
        playerControls = new PlayerMovement();
          
    }
    void Update() {
        moveDirection = move.ReadValue<Vector2>();
 

        if (_isCaught)
        {
            var step = 10f * Time.deltaTime;
            Debug.Log(_rotateTowards);
            // Not working
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotateTowards.normalized), step);
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            // DELETE ON BUILD
            UnityEditor.EditorApplication.isPlaying = false;
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
        if (_isCaught)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        rb.AddForce(mDirection.normalized * playerSpeed, ForceMode.Force);

        
    }

    public void CaughtPlayer(GameObject _guard)
    {
        _isCaught = true;
        this._guard = _guard;
        _rotateTowards = new Vector3(transform.rotation.x, _guard.transform.position.y - transform.position.y, transform.rotation.z);
    }
}
