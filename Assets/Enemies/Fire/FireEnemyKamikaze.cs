using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyKamikaze : MonoBehaviour
{
    private CharacterController controller;
    private float speed = 5f;
    private bool isAttacking;
    private Coroutine attackRoutine;

    public GameObject explosionPrefab;

    private Vector3 jumpVector;
    private Vector3 facingDirection;
    private Vector3 currentVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        facingDirection = transform.forward;
    }

    void FixedUpdate()
    {
        ApplyGravity(Time.fixedDeltaTime);
    }

    // Update is called once per frame
    void Update()
    {       
        if (isAttacking)
        {           
            Vector3 moveDirection = (Player.gameObject.transform.position-transform.position);
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized;

            Vector3 moveVector = new Vector3();
            if (moveDirection.magnitude > 0)
            {
                facingDirection = moveDirection;
            }
            
            moveVector = moveDirection * speed;

            currentVelocity = moveVector + jumpVector;
            controller.Move(currentVelocity * Time.deltaTime);
            RotateEntity(facingDirection, 900f);
        }
    }

    public void RotateEntity(Vector3 newRotation, float speed)
    {
        facingDirection = newRotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newRotation), speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attackRoutine != null)
                StopCoroutine(attackRoutine);

            attackRoutine = StartCoroutine("Explode");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player") && isAttacking)
        {
            Explosion();
            isAttacking = false;
        }
    }

    IEnumerator Explode()
    {
        isAttacking = true;
        yield return new WaitForSeconds(10);

        Explosion();
    }

    private void Explosion()
    {
        GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
        explosion.transform.position = this.transform.position;
        UnityEngine.Object.Destroy(explosion, 10f);
        Destroy(this.gameObject);
        if (Vector3.Distance(Player.gameObject.transform.position, this.transform.position) < 5)
        {
            Player.stats.health -= 100;
        }
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
                jumpVector.y += -9.81f * 2.5f * elapsed;
            }
            else 
            {
                jumpVector.y += -9.81f * elapsed;
            }
        }
    }
}
