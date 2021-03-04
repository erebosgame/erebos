using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Vector3 velocity;
    private Vector3 jumpVector = new Vector3();
    public Vector3 facingDirection { get; private set; }

    private float speed = 7f;
    private float jumpVelocity = 6f;
    private float gravity = -9.81f;
    private float fallMultiplier = 2.5f;
    private float airControl = 0.7f;

    bool isJumping = false;

    void Awake()
    {
        Player.movement = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        facingDirection = transform.forward;
        velocity = new Vector3();
    }

    void FixedUpdate()
    {
        ApplyGravity(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 moveDirection = new Vector3();
        Vector3 moveVector = new Vector3();
        bool jump = false;

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        jump = Input.GetKeyDown(KeyCode.Space);
        isJumping = Input.GetKey(KeyCode.Space);

        if (CanMove()) 
        {
            Vector3 relativeMoveDirection = Vector3.zero;
            if (moveDirection.magnitude > 0)
            {
                relativeMoveDirection = GetRelativeDirection(moveDirection);
            }
            if (controller.isGrounded)
            {
                if (moveDirection.magnitude > 0)
                {
                    facingDirection = relativeMoveDirection;
                }
                if (jump)
                {
                    jumpVector = (relativeMoveDirection * speed * airControl) + Vector3.up * jumpVelocity;
                }
                else
                {
                    moveVector = relativeMoveDirection * speed * (1-airControl);
                }
            }
            else 
            {
                moveVector = relativeMoveDirection * speed * 0.3f;
            }
        }

        controller.Move((moveVector + jumpVector + velocity) * Time.deltaTime);
        RotatePlayer(facingDirection);
    }
    
    public Vector3 GetRelativeDirection(Vector3 direction) 
    {
        return Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * direction;
    }
    public void FaceRelativeDirection(Vector3 direction) 
    {
        facingDirection = GetRelativeDirection(direction);
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
                jumpVector.y += gravity * fallMultiplier * elapsed;
            }
            else 
            {
                jumpVector.y += gravity * elapsed;
            }
        }
    }

    private void RotatePlayer(Vector3 newRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newRotation), 900f * Time.deltaTime);
    }

    bool CanMove()
    {
        return Player.stats.elementState == Element.NoElement;
    }
}