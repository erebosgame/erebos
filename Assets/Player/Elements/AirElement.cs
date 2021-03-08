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
        controller = GetComponent<CharacterController>();
        Player.stats.elementState = Element.Air;
        
        Player.gameObject.GetComponent<CharacterController>().enabled = false;
        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        
        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);

        Player.movement.FaceRelativeDirection(new Vector3(0,0,1));
        initialDirection = Camera.main.transform.forward;
        initialPosition = Player.gameObject.transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = Player.gameObject.transform.position;
        
        if (Vector3.Distance(initialPosition, currentPosition) >= maxDistance)
            End();

        controller.Move(initialDirection * 60f * Time.deltaTime);
    }

    void End() 
    {
        if (ended)
            return;
        ended = true;

        meshRenderer.enabled = true;
        Player.gameObject.GetComponent<CharacterController>().enabled = true;
        Player.stats.elementState = Element.NoElement;
        transform.DetachChildren();
        UnityEngine.Object.Destroy(this.gameObject);
    }
}