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

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
