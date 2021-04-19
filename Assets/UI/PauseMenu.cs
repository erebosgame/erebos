using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject mainUI;
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePaused)
            {
                ResumeGame();
            }
            else 
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        mainUI.SetActive(true);
        Time.timeScale = 1;
        gamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
        mainUI.SetActive(false);
        Time.timeScale = 0;
        gamePaused = true;  
    }

    void LoadTitleScreen()
    {
        Debug.Log("TitleScreen!");
    }

    void SaveGame()
    {
        Debug.Log("Saving...");
    }

    void LoadGame()
    {
        Debug.Log("Loading...");
    }
}
