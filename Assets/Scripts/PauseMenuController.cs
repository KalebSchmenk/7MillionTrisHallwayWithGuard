using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] Button _resumeGameButton;
    [SerializeField] Button _quitGameButton;
    [SerializeField] GameObject _pauseMenuParent;
    public bool paused = false;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenuParent.activeSelf == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    private void PauseGame()
    {
        Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _pauseMenuParent.SetActive(true);
        paused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenuParent.SetActive(false);
        paused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Game quit!");
        Application.Quit();
    }
}
