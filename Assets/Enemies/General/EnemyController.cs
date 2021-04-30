using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;    
    private Vector3 jumpVector;
    private Vector3 moveVector;

    private Vector3 _facingDirection;
    private Vector3 FacingDirection { get {return _facingDirection;} set {_facingDirection = new Vector3(value.x, 0, value.z).normalized;} }
    private Vector3 CurrentVelocity { get {return jumpVector + moveVector;} }

    private float fallMultiplier = 2.5f;
    private float jumpVelocity = 6f;
    private float airControl = 0.7f;

    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        FacingDirection = transform.forward;
        jumpVector = Vector3.zero;
        moveVector = Vector3.zero;
    }

    void FixedUpdate()
    {
        ApplyGravity(Time.fixedDeltaTime);

        // if (isRolling)
        //     Roll(moveVector * Time.deltaTime);
        // else
        RotateSlow(FacingDirection, 900f);
        controller.Move(jumpVector * Time.fixedDeltaTime);
    }
    
    // private void Roll(Vector3 newPos)
    // {
    //     Vector3 direction = newPos.normalized;
    //     this.transform.rotation = (Quaternion.AngleAxis(360*newPos.magnitude/(2*Mathf.PI*0.5f), Vector3.Cross(Vector3.up, direction))) * this.transform.rotation;
    // }

    public void MoveTowards(Vector3 position, float delta)
    {
        LookAt(position);
        Vector3 moveDirection = (position - transform.position);
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized;  

        if (controller.isGrounded)
        {
            moveVector = moveDirection * speed;   
        }
        else
        {
            moveVector = moveDirection * speed * (1-airControl);
        }
        controller.Move(moveVector * delta);
    }

    public void Jump()
    {
        jumpVector = (moveVector * speed * airControl) + Vector3.up * jumpVelocity;
    }

    public void LookAt(Vector3 position)
    {
        Vector3 lookDirection = (position - transform.position);
        FacingDirection = lookDirection;
    }

    private void RotateSlow(Vector3 newRotation, float speed)
    {
        if (newRotation != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newRotation), speed * Time.deltaTime);
    }

    private void ApplyGravity(float elapsed)
    {
        if (controller.isGrounded) 
        {   
            if (jumpVector.y < 0)
            {
                jumpVector.y = -2f;
            }
        }
        else 
        {
            // if (jumpVector.y <= 0 || !isJumping)
            if (jumpVector.y <= 0)
            {
                jumpVector.y += -Physics.gravity.magnitude * fallMultiplier * elapsed;
            }
            else 
            {
                jumpVector.y += -Physics.gravity.magnitude * elapsed;
            }
        }
    }
}
