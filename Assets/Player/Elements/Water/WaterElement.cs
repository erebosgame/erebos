using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class WaterElement : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    CharacterController controller;

    Camera mainCamera;
    
    public Vector3 facingDirection { get; private set; }
    public Vector3 currentVelocity { get; private set; }

    private float speed = 7f;
    private float gravity = -9.81f;

    private Vector3 jumpVector = new Vector3();

    void Start()
    {
        mainCamera = Camera.main;
        speed = 7f;
        Player.stats.elementState = Element.Water;
        Player.gameObject.GetComponent<CharacterController>().enabled = false;

        controller = this.GetComponent<CharacterController>();

        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        Player.gameObject.transform.localPosition = Vector3.zero;
        StartCoroutine("EndAfterTime");
    }

    void Update()
    { 
        Vector3 moveDirection = new Vector3();
        Vector3 moveVector = new Vector3();

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        Vector3 relativeMoveDirection = Vector3.zero;

        if (moveDirection.magnitude > 0)
        {
            relativeMoveDirection = GetRelativeDirection(moveDirection);
        }
        if(controller.isGrounded) {}
        else 
        {
            moveVector = relativeMoveDirection * speed;
        }

        moveVector = relativeMoveDirection * speed;
        
        Move((moveVector + jumpVector) * Time.deltaTime);
        ApplyGravity(Time.deltaTime);
    }

    
    private void ApplyGravity(float elapsed)
    {
        if (controller.isGrounded) 
        {   
            if (jumpVector.y < 0)
            {
                jumpVector = Vector3.zero + Vector3.up * -2f;
            }
        }
        else 
        {
            // if (jumpVector.y <= 0 || !isJumping)
            if (jumpVector.y <= 0)
            {
                jumpVector.y += gravity * 2.5f * elapsed;
            }
            else 
            {
                jumpVector.y += gravity * elapsed;
            }
        }
    }

    public void Move(Vector3 vector)
    {
        controller.Move(vector);
    }
    
    public Vector3 GetRelativeDirection(Vector3 direction) 
    {
        return Quaternion.LookRotation(new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z)) * direction;
    }
    public void FaceRelativeDirection(Vector3 direction) 
    {
        facingDirection = GetRelativeDirection(direction);
    }
    
    public void RotatePlayer(Vector3 newRotation, float speed)
    {
        facingDirection = newRotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newRotation), speed * Time.deltaTime);
    }


    public void RotatePlayer(float degrees)
    {
        facingDirection = Quaternion.Euler(0,degrees,0) * facingDirection;
        transform.forward = facingDirection;
    }

    IEnumerator EndAfterTime()
    {
        yield return new WaitForSeconds(10f);
        End();
    }

    public void End() 
    {
        transform.DetachChildren();
        Player.gameObject.GetComponent<CharacterController>().enabled = true;
        Player.gameObject.transform.rotation = Quaternion.identity;
        Player.stats.elementState = Element.NoElement;
        meshRenderer.enabled = true;
        UnityEngine.Object.Destroy(this.gameObject);
    }
}