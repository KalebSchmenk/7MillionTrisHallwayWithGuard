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

    void Awake() {
        playerControls = new PlayerMovement();
        rb = GetComponent<Rigidbody>();    
    }
    void Update() {
        moveDirection = move.ReadValue<Vector2>();
    }
    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();
    }

    private void OnDisable() {
        move.Disable();
    }


    private void FixedUpdate() {
        rb.velocity = new Vector3(moveDirection.x * playerSpeed, rb.velocity.y, moveDirection.y * playerSpeed);
    }
}
