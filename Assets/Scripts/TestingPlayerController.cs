using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayerController : MonoBehaviour
{
    private CharacterController _char;
    public float speed = 0.02f;

    public bool _playerCaptured = false;
    void Start()
    {
        _char = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (_playerCaptured == false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                _char.Move(Vector3.forward * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                _char.Move(Vector3.back * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                _char.Move(Vector3.left * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                _char.Move(Vector3.right * speed);
            }
        }
    }
}
