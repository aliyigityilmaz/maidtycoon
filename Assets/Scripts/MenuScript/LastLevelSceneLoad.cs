using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLevelSceneLoad : MonoBehaviour
{
    public GameObject lastBlackScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LastLevel()
    {
        lastBlackScreen.SetActive(true);
    }
}
