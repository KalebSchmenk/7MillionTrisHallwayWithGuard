using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour{
    public PlayerMovement playerControls;
    Vector2 moveDirection;

    [SerializeField] float playerSpeed = 10.0f;

    public Rigidbody rb;

    private InputAction move;

    public bool hasCrown = false;
    private bool _isCaught = false;
    private GameObject _guard;
    private Vector3 _rotateTowards;

    void Awake() {
        playerControls = new PlayerMovement();
        rb = GetComponent<Rigidbody>();    
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
        rb.velocity = new Vector3(moveDirection.x * playerSpeed, rb.velocity.y, moveDirection.y * playerSpeed);
    }

    public void CaughtPlayer(GameObject _guard)
    {
        _isCaught = true;
        this._guard = _guard;
        _rotateTowards = new Vector3(transform.rotation.x, _guard.transform.position.y - transform.position.y, transform.rotation.z);
    }
}
