using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    Camera mainCamera;

    private Vector3 jumpVector = new Vector3();
    public Vector3 facingDirection { get; private set; }
    public Vector3 currentVelocity { get; private set; }

    private float speed = 7f;
    private float jumpVelocity = 6f;
    private float fallMultiplier = 2.8f;
    private float airControl = 0.7f;

    private bool isGliding = false;

    bool isJumping = false;

    public GameObject glider;

    void Awake()
    {
        Player.movement = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        mainCamera = Camera.main;
        facingDirection = transform.forward;
        Cursor.lockState = CursorLockMode.Locked;
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
                    jumpVector = (relativeMoveDirection * speed * (isGliding ? 0f : airControl)) + Vector3.up * jumpVelocity;
                }
                else
                {
                    moveVector = relativeMoveDirection * speed;
                }
            }
            else 
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ActivateGlider();
                }

                moveVector = relativeMoveDirection * (isGliding ? 5.5f :speed) * (1- (isGliding ? 0f : airControl));
            }

            currentVelocity = moveVector + jumpVector;
            Move(currentVelocity * Time.deltaTime);
            if(Player.stats.weapon)
                RotatePlayer(facingDirection, 900f);
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
    
    private void ApplyGravity(float elapsed)
    {
        if (controller.isGrounded) 
        {
            isGliding = false;
            glider.SetActive(isGliding);

            if (jumpVector.y < 0)
            {
                jumpVector = Vector3.zero + Vector3.up * -2f;
            }
        }
        else //is falling 
        {
            // if (jumpVector.y <= 0 || !isJumping)
            if (jumpVector.y <= 0)
            {
                if(isGliding)
                {
                    jumpVector.y += -Physics.gravity.magnitude * 0.09f * elapsed;

                }
                else
                {
                    jumpVector.y += -Physics.gravity.magnitude * fallMultiplier * elapsed;
                }
            }
            else 
            {
                jumpVector.y += -Physics.gravity.magnitude * elapsed;
            }
        }
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

    bool CanMove()
    {
        return Player.stats.elementState == Element.NoElement;
    }

    private void ActivateGlider()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        if (hit.distance > 4)
        {
            isGliding = !isGliding;
            glider.SetActive(isGliding);
            jumpVector = Vector3.zero;
        }
    }
}