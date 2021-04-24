using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAimController : MonoBehaviour
{
    public GameObject aimReticle;
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
        if(!Player.stats.weapon && !CameraLogic.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming"))
        {
            print(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
            playerMovement.RotatePlayer(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
            // TODO
            // cameraRotator.transform.forward = (Player.gameObject.transform.position - normalCamera.transform.position).normalized;
            cameraRotator.SetActive(true);
            CameraLogic.instance.animator.SetTrigger("startaiming");


            StartCoroutine("ShowReticle");
        }
        else if(Player.stats.weapon && CameraLogic.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming"))
        {        
            CameraLogic.instance.animator.SetTrigger("stopaiming");
        }
    }

    IEnumerator ShowReticle()
    {
        yield return new WaitForSeconds(0.25f);
        aimReticle.SetActive(true);
    }
}


