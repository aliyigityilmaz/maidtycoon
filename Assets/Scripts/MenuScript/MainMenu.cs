using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject creditsMenu;
    [SerializeField] private bool creditsActive;

    public GameObject fadeScreen;
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        creditsMenu.SetActive(false);
        creditsActive = false;
    }
    public void PlayGame()
    {
        fadeScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Credits()
    {
        if(creditsActive)
        {
            creditsMenu.SetActive(false);
            creditsActive = false;
        }
        else
        {
            creditsMenu.SetActive(true);
            creditsActive = true;
        }
    }
}

