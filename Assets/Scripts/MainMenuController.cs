using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsMenu;
    public GameObject mainFirst;
    public GameObject credFirst;
    public GameObject credBack;
    public AudioSource menuSounds;
    public AudioClip buttonPress;

    

    
    

    private void Start()
    {

        
        Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        StartCoroutine(SoundBeforePlay());
        
    }

    public void ShowCredits()
    {
        menuSounds.PlayOneShot(buttonPress);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(credFirst);
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        menuSounds.PlayOneShot(buttonPress);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(credBack);
        _creditsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        StartCoroutine(SoundBeforeQuit());
    }

    void Awake(){
        
    }

     private void OnEnable(){

    }

    private void OnDisable(){
 
    }

    private void Update() {
        
    }

    private IEnumerator SoundBeforePlay(){
        menuSounds.PlayOneShot(buttonPress);
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene("CastleLevel");
    }

        private IEnumerator SoundBeforeQuit(){
        menuSounds.PlayOneShot(buttonPress);
        yield return new WaitForSecondsRealtime(0.3f);
        Application.Quit();
    }

   
}
