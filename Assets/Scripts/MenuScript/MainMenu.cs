using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject creditsMenu;
    [SerializeField] private bool creditsActive;

    private void Start()
    {
        creditsMenu.SetActive(false);
        creditsActive = false;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
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

