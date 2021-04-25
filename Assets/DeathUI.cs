using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{   
    public static DeathUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public static void SetActive(bool value)
    {
        instance.gameObject.SetActive(value);
    }

    public void Respawn()
    {
        CameraLogic.instance.animator.SetTrigger("respawn");
        Player.gameObject.SetActive(true);

        MenuLogic.Update(MenuState.Playing);
        
        Cursor.lockState = CursorLockMode.Locked;
        Player.stats.Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
