using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTriggerController : MonoBehaviour
{
    [SerializeField] GameObject winUIParent;
    [SerializeField] float _sendToMenuIn = 2.5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerScript = other.gameObject.GetComponent<playerController>();

            if (playerScript.hasCrown == true)
            {
                Debug.Log("Game win! Implement win game functionality");

                winUIParent.SetActive(true);
                StartCoroutine(AwaitSendToMainMenu());
            }
        }
    }

    private IEnumerator AwaitSendToMainMenu()
    {
        yield return new WaitForSeconds(_sendToMenuIn);

        SceneManager.LoadScene("MainMenu");
    }
}
