using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class AirElement : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private CharacterController controller;
    
    private Vector3 initialPosition;
    private Vector3 initialDirection;

    private float maxDistance = 10f;
    
    private bool ended;

    void Start()
    {
        if (Player.movement.isGliding)
            Player.movement.ActivateGlider();
            
        controller = GetComponent<CharacterController>();
        Player.stats.elementState = Element.Air;
        
        Player.gameObject.GetComponent<CharacterController>().enabled = false;
        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        Player.stats.ToggleRenderer(false);

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        Player.elementGameObject = this.gameObject;

        Player.movement.FaceRelativeDirection(new Vector3(0,0,1));
        initialDirection = Camera.main.transform.forward;
        initialPosition = Player.gameObject.transform.position;

        Bonk.instance.gameObject.SetActive(true);
        StartCoroutine("EndAfterTime");
    }

    void Update()
    {
        Vector3 currentPosition = Player.gameObject.transform.position;
        
        if (Vector3.Distance(initialPosition, currentPosition) >= maxDistance)
            End();

        controller.Move(initialDirection * 60f * Time.deltaTime);
    }

    IEnumerator EndAfterTime()
    {
        yield return new WaitForSeconds(5f);
        End();
    }

    void End() 
    {
        if (ended)
            return;
        ended = true;

        Player.stats.ToggleRenderer(true);
        Player.gameObject.GetComponent<CharacterController>().enabled = true;
        Player.stats.elementState = Element.NoElement;
        this.GetComponentInChildren<ParticleSystem>().Stop(true);
        while (transform.childCount > 0) 
        {
            transform.GetChild(0).parent = transform.parent;
        }
        Player.elementGameObject = null;
        Bonk.instance.gameObject.SetActive(true);

        UnityEngine.Object.Destroy(this.gameObject);
    }
}