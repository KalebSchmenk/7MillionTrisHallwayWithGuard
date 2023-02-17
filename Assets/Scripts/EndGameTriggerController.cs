using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTriggerController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerScript = other.gameObject.GetComponent<playerController>();

            if (playerScript.hasCrown == true)
            {
                Debug.Log("Game win! Implement win game functionality");
            }
        }
    }
}
