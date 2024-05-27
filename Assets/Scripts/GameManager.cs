using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public List<GameObject> chairPoints;
    public List<GameObject> busyChairs;

    public List<GameObject> seatedCustomer = new List<GameObject>();

    public UnityEvent OnFoodAte;

    public GameObject pausePanel;
    
    public bool gamePaused = false;

    public GameObject optionsMenu;

    public bool isGameStarted = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Level1")
        {
            isGameStarted = false;
        }
        else
        {
            StartGame();
        }
        Time.timeScale = 1;
        UpdateChairList();
        pausePanel.SetActive(false);
    }
    // Update is called once per frame
    public void AddToList(GameObject customer)
    {
        seatedCustomer.Add(customer);
    }

    private void OnEnable()
    {
        OnFoodAte.AddListener(OnFoodAteHandler);
    }

    private void OnDisable()
    {
        OnFoodAte.RemoveListener(OnFoodAteHandler);
    }

    private void OnFoodAteHandler()
    {
        throw new NotImplementedException();
    }

    public void UpdateChairList()
    {
        foreach (GameObject chair in GameObject.FindGameObjectsWithTag("Sit"))
        {
            if (!chairPoints.Contains(chair) && !busyChairs.Contains(chair))
            {
                chairPoints.Add(chair);
            }
        }
    }

    public GameObject GetRandomChair()
    {
        int randomIndex = UnityEngine.Random.Range(0, chairPoints.Count);
        GameObject chair = chairPoints[randomIndex];
        chair.GetComponent<Chair>().isOccupied = true;
        busyChairs.Add(chair);
        chairPoints.Remove(chair);
        return chair;
    }

    public void RemoveChair(GameObject chair)
    {
        chair.GetComponent<Chair>().isOccupied = false;
        busyChairs.Remove(chair);
        chairPoints.Add(chair);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        if (gamePaused)
        {
            pausePanel.SetActive(false);
            gamePaused = false;
        }
        else
        {
            pausePanel.SetActive(true);
            gamePaused = true;
        }
    }

    public void OptionsMenu()
    {
        if (optionsMenu.activeSelf == false)
        {
            optionsMenu.SetActive(true);
        }
        else
        {
            optionsMenu.SetActive(false);
        }
    }

    public void StartGame()
    {
        isGameStarted = true;
    }

}
