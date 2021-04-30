using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    private enum State {
        Idle,
        ChaseTarget,
        Attack,
        Escape,
    }

    private State state;
    private EnemyController controller;
    private Enemy enemy;
    // private bool isAttacking;
    private bool isMovingIdle;
    private Vector3 destination;
    Vector2 direction;
    private Vector3 spawnPosition;

    private float attackTimer;

    private float attackDistance = 3f;

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = this.GetComponent<EnemyController>();
        enemy = this.GetComponent<Enemy>();
        state = State.Idle;
        isMovingIdle = false;
        spawnPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case State.Idle:
                controller.speed = 3;
                if (!isMovingIdle)
                {
                    direction = Random.insideUnitCircle.normalized * Random.Range(7f,10f);
                    destination = spawnPosition + new Vector3(direction.x, 0, direction.y);
                    isMovingIdle = true;
                }
                else
                {
                    Debug.Log(Vector3.Distance(this.gameObject.transform.position, destination));
                    if (Vector3.Distance(this.gameObject.transform.position, destination) < 5f)
                    {
                        isMovingIdle = false;
                    }
                }
                controller.MoveTowards(destination, Time.deltaTime);
                break;
            case State.ChaseTarget:
                controller.speed = 7;
                controller.MoveTowards(Player.gameObject.transform.position, Time.deltaTime);

                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < attackDistance)
                {   
                    state = State.Attack;
                    attackTimer = Time.time + 0.35F;
                }
                break;
            case State.Attack:
                // isAttacking = true;
                if (enemy.Health < 15)
                {
                    state = State.Escape;
                }
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) > attackDistance)
                {   
                    state = State.ChaseTarget;
                }
                if (Time.time > attackTimer)
                {
                    Attack();
                }
                controller.LookAt(Player.gameObject.transform.position);
            break;
            case State.Escape:
                controller.speed = 11.5f;
                direction = (Player.gameObject.transform.position - this.transform.position);
                direction.y = 0;
                direction = direction.normalized;
                controller.MoveTowards(direction, Time.deltaTime);
            break;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && state == State.Idle)
        {
            //if (attackRoutine != null)
           //     StopCoroutine(attackRoutine); 

            //attackRoutine = StartCoroutine("FireSling");
            state = State.ChaseTarget;
            // isAttacking = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && state != State.Escape)
        {
            //StopCoroutine(attackRoutine);
            state = State.Idle;
            // isAttacking = false;
        }
    }

/*    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player") && isAttacking)
        {
            Explosion();
            isAttacking = false;
        }
    } */

    void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position - this.transform.forward, attackDistance, LayerMask.GetMask("Player"));
        if (colliders.Length > 0 && Mathf.Abs(Vector3.Angle(transform.forward, (Player.gameObject.transform.position-this.transform.position).normalized)) < 30)
        {
            Player.stats.TakeDamage(10);  
            attackTimer = Time.time + 2F;
            animator.SetTrigger("BaseAttack");  
        }        
        //yield return new WaitForSeconds(20F);
    }

    public void OnAttackEnd() {}
}
