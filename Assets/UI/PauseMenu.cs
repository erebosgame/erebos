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

    public void LoadTitleScreen()
    {
        Debug.Log("TitleScreen!");
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("FireElement", Player.stats.unlockedElements[Element.Fire] ? 1 : 0);
        PlayerPrefs.SetInt("EarthElement", Player.stats.unlockedElements[Element.Earth] ? 1 : 0);
        PlayerPrefs.SetInt("AirElement", Player.stats.unlockedElements[Element.Air] ? 1 : 0);
        PlayerPrefs.SetInt("WaterElement", Player.stats.unlockedElements[Element.Water] ? 1 : 0);


        PlayerPrefs.SetInt("FireDefeated", Player.stats.defeatedBosses.Contains(Element.Fire) ? 1 : 0);
        PlayerPrefs.SetInt("EarthDefeated", Player.stats.defeatedBosses.Contains(Element.Earth) ? 1 : 0);
        PlayerPrefs.SetInt("AirDefeated", Player.stats.defeatedBosses.Contains(Element.Air) ? 1 : 0);
        PlayerPrefs.SetInt("WaterDefeated", Player.stats.defeatedBosses.Contains(Element.Water) ? 1 : 0);

        PlayerPrefs.SetFloat("FireBossDeathAngle", FireBoss.instance.transform.parent.transform.localEulerAngles.y);

        PlayerPrefs.SetFloat("PosX", Player.gameObject.transform.position.x);
        PlayerPrefs.SetFloat("PosY", Player.gameObject.transform.position.y);
        PlayerPrefs.SetFloat("PosZ", Player.gameObject.transform.position.z);


        PlayerPrefs.Save();
        Debug.Log("Saving...");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
