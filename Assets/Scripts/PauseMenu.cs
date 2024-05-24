using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    //public GameObject settingsPanel;
    //public bool settingsPanelOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Continue()
    {
        pausePanel.SetActive(false);
    }
    //public void Settings()
    //{
    //    if (settingsPanelOpen)
    //    {
    //        settingsPanel.SetActive(false);
    //        settingsPanelOpen = false;
    //    }
    //    else
    //    {
    //        settingsPanel.SetActive(true);
    //        settingsPanelOpen = true;
    //    }

    //}
}
