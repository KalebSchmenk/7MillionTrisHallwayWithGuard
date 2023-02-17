using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrownController : MonoBehaviour
{
    private float _yVar;
    private float _startY;

    [SerializeField] private float bounceSpeed = 0.15f;
    [SerializeField] private float bounceHeight = 0.2f;
    [SerializeField] private float rotateSpeed = 0.15f;

    private void Start()
    {
        _startY = transform.position.y;
    }

    void Update()
    {
        _yVar = Mathf.PingPong(Time.time * bounceSpeed, bounceHeight);
        transform.position = new Vector3(transform.position.x, _yVar + _startY, transform.position.z);

        transform.Rotate(0.0f, rotateSpeed, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerScript = other.gameObject.GetComponent<playerController>();

            playerScript.hasCrown = true;
            // Instantiate particle effect?

            Destroy(this.gameObject);
        }
    }
}
