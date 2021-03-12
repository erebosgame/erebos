using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAimController : MonoBehaviour
{
    public GameObject aimReticle;

    public GameObject aimCamera;
    public GameObject normalCamera;

    public GameObject cameraRotator;
    //TMP
    PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = Player.gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Player.stats.weapon && !aimCamera.activeInHierarchy)
        {
            playerMovement.RotatePlayer(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z),900f);
            normalCamera.SetActive(false);
            aimCamera.SetActive(true);
            cameraRotator.SetActive(true);


            StartCoroutine("ShowReticle");
        }
        else if(Player.stats.weapon && !normalCamera.activeInHierarchy)
        {
            normalCamera.SetActive(true);
            aimCamera.SetActive(false);
            aimReticle.SetActive(false);
            cameraRotator.SetActive(false);
        }
    }

    IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
        aimReticle.SetActive(true);
    }
}


