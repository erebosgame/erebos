using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Vector3 velocity;
    public Vector3 facingDirection { get; private set; }

    float speed = 10f;
    float jumpVelocity = 5f;
    float gravity = -9.81f;

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
        ApplyGravity(Time.fixedDeltaTime, gravity);
    }

    // Update is called once per frame
    void Update()
    { 
        Vector3 moveDirection = new Vector3();
        bool jump = false;

        if (CanMove()) 
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            jump = Input.GetKeyDown(KeyCode.Space);
        }

        if (controller.isGrounded && jump)
        {
            Jump(jumpVelocity);
        }

        Vector3 relativeMoveDirection = new Vector3();
        if (moveDirection.magnitude > 0)
        {
            FaceRelativeDirection(moveDirection);
            relativeMoveDirection = facingDirection;
        }

        controller.Move((relativeMoveDirection * speed + velocity) * Time.deltaTime);
        RotatePlayer(facingDirection);
    }
    
    public void FaceRelativeDirection(Vector3 direction) 
    {
        facingDirection = Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * direction;
    }
    
    private void ApplyGravity(float elapsed, float acceleration)
    {
        velocity.y += acceleration * elapsed;
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void Jump(float v) 
    {
        velocity.y = v;
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