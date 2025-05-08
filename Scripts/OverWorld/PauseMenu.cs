using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false; 
    public GameObject pauseMenuUI; 
    public GameObject itemMenuUI;  
    public Inventory inventory; 
    public PartyManager partyManager; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume(); 
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; 
        GameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        GameIsPaused = true;
    }

    public void OpenItemMenu()
    {
        pauseMenuUI.SetActive(false);
        itemMenuUI.SetActive(true);
    }

    public void LeaveItemMenu()
    {
        pauseMenuUI.SetActive(true);
        itemMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
