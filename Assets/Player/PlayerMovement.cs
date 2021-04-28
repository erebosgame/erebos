using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LiquidState {
    Water,
    Lava, 
    Air,
}
class PlayerMovement : MonoBehaviour
{
    CharacterController controller;

    public Vector3 jumpVector = new Vector3();
    public Vector3 facingDirection { get; private set; }
    public Vector3 currentVelocity { get; private set; }

    private LiquidState liquid;

    private float speed;
    private float speedNormal = 10f;
    private float speedWaterMultiplier = 0.5f;
    private float speedLavaMultiplier = 0.25f;

    private float jumpVelocity;
    private float jumpVelocityNormal = 6f;
    private float jumpVelocityWaterMultiplier = 0.9f;
    private float jumpVelocityLavaMultiplier = 0.8f;
    
    private float fallMultiplier;
    private float fallMultiplierNormal = 2.8f;
    private float fallMultiplierWaterMultiplier = 0.03f;
    private float fallMultiplierLavaMultiplier = 0.01f;

    private float airControl = 0.7f;
    public bool isGliding = false;
    private float glidingGravity = 0.055f;
    private float glidingSpeed = 12f;
    bool isJumping = false;

    public GameObject glider;
    public GameObject spawnpoint;
    public Cinemachine.CinemachineFreeLook thirdPersonCamera;

    void Awake()
    {
        Player.movement = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        facingDirection = transform.forward;

        speed = speedNormal;
        jumpVelocity = jumpVelocityNormal;
        fallMultiplier = fallMultiplierNormal;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        ApplyGravity(Time.fixedDeltaTime);

        Collider found = null;
        Collider[] colliders = Physics.OverlapCapsule(controller.bounds.min-Vector3.down*0.5f, controller.bounds.max-Vector3.up*0.5f, 0.5f, LayerMask.GetMask("Liquids"), QueryTriggerInteraction.Collide);
        RaycastHit hit;
        Physics.Raycast(this.transform.position, Vector3.down, out hit, 1.1f, LayerMask.GetMask("Liquids"), QueryTriggerInteraction.Collide);

        if (colliders.Length > 0)
        {
            found = colliders[0];
        }
        else if (hit.collider != null)
        {
            found = hit.collider;
        }

        if (found != null)
        {
            if (found.CompareTag("Water")) 
            {
                liquid = LiquidState.Water;
                speed = speedNormal * speedWaterMultiplier;
                jumpVelocity = jumpVelocityNormal * jumpVelocityWaterMultiplier;
                fallMultiplier = fallMultiplierNormal * fallMultiplierWaterMultiplier;
            }
            else if (found.CompareTag("Lava"))
            {
                liquid = LiquidState.Lava;
                speed = speedNormal * speedLavaMultiplier;
                jumpVelocity = jumpVelocityNormal * jumpVelocityLavaMultiplier;
                fallMultiplier = fallMultiplierNormal * fallMultiplierLavaMultiplier;
                Player.stats.TakeDamage(10f*Time.fixedDeltaTime);
            }  
        }
        else 
        {  
            liquid = LiquidState.Air;
            speed = speedNormal;
            jumpVelocity = jumpVelocityNormal;
            fallMultiplier = fallMultiplierNormal;
        }
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

                moveVector = relativeMoveDirection * (isGliding ? glidingSpeed : speed) * (1- (isGliding ? 0f : airControl));
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
        return Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * direction;
    }

    public void FaceRelativeDirection(Vector3 direction) 
    {
        facingDirection = GetRelativeDirection(direction);
    }
    
    public void Teleport(Vector3 position, Quaternion rotation)
    {
        controller.enabled = false;
        transform.position = position;
        transform.rotation = rotation;
        controller.enabled = true;
    }

    public void GotoSpawnpoint()
    {
        Teleport(spawnpoint.transform.position, spawnpoint.transform.rotation);
        thirdPersonCamera.m_XAxis.Value = 140f;
        thirdPersonCamera.m_YAxis.Value = 0.3f;
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
                    jumpVector.y += -Physics.gravity.magnitude * glidingGravity * elapsed;

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

    public void RotatePlayer(Vector3 newRotation)
    {
        facingDirection = newRotation;
        transform.rotation =  Quaternion.LookRotation(newRotation);
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
        if (hit.collider == null     || hit.distance > 4)
        {
            isGliding = !isGliding;
            glider.SetActive(isGliding);
            jumpVector = Vector3.zero;
        }
    }

    public void PushPlayer(Vector3 direction, float force)
    {
        jumpVector += direction.normalized * force;
    }
}