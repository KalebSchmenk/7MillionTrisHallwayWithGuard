using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public PlayerMovement playerControls;
    private InputAction pause;

    [SerializeField] Button _resumeGameButton;
    [SerializeField] Button _quitGameButton;
    [SerializeField] GameObject _pauseMenuParent;
    public bool _paused = false;

    public bool _playerCaught = false;
    

    void Awake(){
        playerControls = new PlayerMovement();
    }

    private void Update()
    {


        if (pause.triggered)
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
        if (_playerCaught) return;

        Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _pauseMenuParent.SetActive(true);
        _paused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenuParent.SetActive(false);
        _paused = false;
    }

    public void QuitGame()
    {
        Debug.Log("Game quit!");
        //Application.Quit();
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable(){
        pause = playerControls.Player.Pause;
        pause.Enable();
    }

    private void OnDisable(){
        pause.Disable();
    }


}
