using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsMenu;

    private void Start()
    {
        Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CastleLevel");
    }

    public void ShowCredits()
    {
        _mainMenu.SetActive(false);
        _creditsMenu.SetActive(true);
    }

    public void BackToMain()
    {
        _creditsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
