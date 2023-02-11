using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayerController : MonoBehaviour
{
    private CharacterController _char;
    public float speed = 0.02f;
    void Start()
    {
        _char = GetComponent<CharacterController>();
    }

    void Update()
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
