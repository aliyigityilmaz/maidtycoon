using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelAfterDialogue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToNextLevelAfterDialogue()
    {
        int sceneNumber = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneNumber + 1);
    }
}
