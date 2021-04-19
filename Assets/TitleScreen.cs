using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject thirdPersonCamera;
    public GameObject titleUI;
    public GameObject mainUI;
    public GameObject pauseUI;
    // Start is called before the first frame update
    void Start()
    {
        thirdPersonCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayGame()
    {
        Debug.Log("Play");
        titleUI.SetActive(false);
        mainUI.SetActive(true);
        thirdPersonCamera.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
