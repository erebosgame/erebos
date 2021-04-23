using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public GameObject menuCamera;
    public GameObject titleUI;
    public GameObject mainUI;
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        menuCamera.SetActive(true);
        thirdPersonCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        Debug.Log("Play");
        titleUI.SetActive(false);
        thirdPersonCamera.SetActive(true);
        menuCamera.SetActive(false);
        mainUI.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void LoadGame()
    {
        Debug.Log("Load");
        bool fireboss = PlayerPrefs.GetInt("FireDefeated", 0) != 0;
        bool earthboss = PlayerPrefs.GetInt("EarthDefeated", 0) != 0;
        bool airboss = PlayerPrefs.GetInt("AirDefeated", 0) != 0;
        bool waterboss = PlayerPrefs.GetInt("WaterDefeated", 0) != 0;
        
        bool fireelement = PlayerPrefs.GetInt("FireElement", 0) != 0;
        bool earthelement = PlayerPrefs.GetInt("EarthElement", 0) != 0;
        bool airelement = PlayerPrefs.GetInt("AirElement", 0) != 0;
        bool waterelement = PlayerPrefs.GetInt("WaterElement", 0) != 0;

        int health = PlayerPrefs.GetInt("Health", 100);
        float posx = PlayerPrefs.GetFloat("PosX", 0);
        float posy = PlayerPrefs.GetFloat("PosY", 0);
        float posz = PlayerPrefs.GetFloat("PosZ", 0);

        if (fireboss)
        {
            float firebossangle = PlayerPrefs.GetFloat("FireBossDeathAngle");
            FireBoss.LoadKill(firebossangle);
            if (fireelement)
                FireElementItem.LoadElement();
        }

        int airbossphase = PlayerPrefs.GetInt("CurrentAirPhase");
        AirBoss.LoadPhase(airbossphase);
        if (airelement)
            AirElementItem.LoadElement();
        
        Player.stats.health = health;
        Player.gameObject.GetComponent<CharacterController>().enabled = false;
        Player.gameObject.transform.position = new Vector3(posx,posy,posz);
        Player.gameObject.GetComponent<CharacterController>().enabled = true;

        PlayGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
