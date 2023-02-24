using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public PlayerMovement playerControls;
    private InputAction pause;

    [SerializeField] Button _resumeGameButton;
    [SerializeField] Button _quitGameButton;
    [SerializeField] GameObject _pauseMenuParent;
    public bool _paused = false;

    public bool _playerCaught = false;

    public GameObject selectedPause;

    public GameObject playerWalkObject;
    public GameObject playerSprintObject;
    public AudioSource playerWalkSource;
    public AudioSource playerSprintSource;
    public GameObject backgroundMusic;
    public GameObject chaseMusic;
    public GameObject menuSoundsObject;

    

    public AudioSource menuSounds;
    public AudioClip buttonPress;

  


   
    

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
        if(_playerCaught){
            menuSoundsObject.SetActive(false);
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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectedPause);
        playerWalkSource.mute = true;
        playerSprintSource.mute = true;
        playerWalkObject.SetActive(false);
        playerSprintObject.SetActive(false);
        backgroundMusic.SetActive(false);
        chaseMusic.SetActive(false);
        
    }

    public void ResumeGame()
    {
        
        menuSounds.PlayOneShot(buttonPress);
        Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        _pauseMenuParent.SetActive(false);
        _paused = false;
        playerWalkObject.SetActive(true);
        playerSprintObject.SetActive(true);
        backgroundMusic.SetActive(true);
        chaseMusic.SetActive(true);
        
        
    }

    public void QuitGame()
    {
        StartCoroutine(SoundBeforeMainMenu());
        Debug.Log("Game quit!");
        //Application.Quit();
        
   
    }

    private void OnEnable(){
        pause = playerControls.Player.Pause;
        pause.Enable();
    }

    private void OnDisable(){
        pause.Disable();
    }

    private IEnumerator SoundBeforeMainMenu(){
        menuSounds.PlayOneShot(buttonPress);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene("MainMenu");
    }




}
