using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrower : MonoBehaviour
{
    private enum State {
        Idle,
        ChaseTarget,
        Attack,
        Escape,
        Return
    }

    private State state;
    private EnemyController controller;
    private Enemy enemy;
    // private bool isAttacking;
    private bool isMovingIdle;
    public GameObject ammo;
    private Vector3 destination;
    Vector2 direction;
    private Vector3 spawnPosition;

    private float shootTimer;

    public GameObject eye;

    private float attackRange = 60F;
    private float escapeRange = 20F;

    private float lastIdlePos;


    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<EnemyController>();
        enemy = this.GetComponent<Enemy>();
        state = State.Idle;
        isMovingIdle = false;
        spawnPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        switch (state) {
            case State.Idle:
                if (!isMovingIdle || Time.time > lastIdlePos + 20f)
                {
                    direction = Random.insideUnitCircle.normalized * Random.Range(7f,10f);
                    destination = spawnPosition + new Vector3(direction.x, 0, direction.y);
                    Physics.Raycast(destination, Vector3.down, out hit, 500f, LayerMask.GetMask("Terrain"));
                    if (hit.collider == null)
                        Physics.Raycast(destination, Vector3.up, out hit, 500f, LayerMask.GetMask("Terrain"));
                    if (hit.collider == null)
                        break;
                    destination = hit.point;
                    lastIdlePos = Time.time;
                    isMovingIdle = true;
                }
                else
                {
                    if (Vector3.Distance(this.gameObject.transform.position, destination) < 5f && isMovingIdle)
                    {
                        isMovingIdle = false;
                    }
                }
                controller.MoveTowards(destination, Time.deltaTime);
                break;
            case State.ChaseTarget:
                controller.MoveTowards(Player.gameObject.transform.position, Time.deltaTime);

                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < attackRange)
                {   
                    state = State.Attack;
                    shootTimer = Time.time + 1f;
                }
                else if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < escapeRange)
                {   
                    state = State.Escape;
                }
                else if (Vector3.Distance(this.gameObject.transform.position, spawnPosition) > 70F)
                {
                    state = State.Return;
                }
                 break;
            case State.Attack:
                // isAttacking = true;
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < escapeRange)
                {   
                    state = State.Escape;
                }
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) > attackRange * 1.2)
                {   
                    state = State.ChaseTarget;
                }
                if (Time.time > shootTimer)
                {
                    FireSling();
                    shootTimer = Time.time + 2F;
                }
                controller.LookAt(Player.gameObject.transform.position);
                break;
            case State.Escape:
                // isAttacking = false;
                controller.speed = 15;
                if (!isMovingIdle)
                {
                    direction = (Player.gameObject.transform.position - this.transform.position) * -1* Random.Range(10f,7f);
                    destination = this.gameObject.transform.position + new Vector3(direction.x, 0, direction.y);
                    isMovingIdle = true;
                }
                else
                {
                    if (Vector3.Distance(this.gameObject.transform.position, destination) < 8f && isMovingIdle)
                    {
                        isMovingIdle = false;
                    }
                }
                controller.MoveTowards(destination, Time.deltaTime);
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) >= attackRange) 
                {   
                    controller.speed = 5;
                    state = State.Attack;
                }
                break;
            case State.Return:
                controller.MoveTowards(spawnPosition, Time.deltaTime);
                if (Vector3.Distance(this.gameObject.transform.position, spawnPosition) < 5f)
                {
                    state = State.Idle;
                }
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //if (attackRoutine != null)
           //     StopCoroutine(attackRoutine); 

            //attackRoutine = StartCoroutine("FireSling");
            state = State.Attack;
            // isAttacking = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //StopCoroutine(attackRoutine);
            state = State.ChaseTarget;
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

    void FireSling()
    {
        GameObject projectile = Instantiate(ammo, eye.transform.position, Quaternion.identity);

        Vector3 direction = (Player.gameObject.transform.position - projectile.transform.position).normalized;

        projectile.transform.forward = direction;

        projectile.GetComponent<FireBall>().Fire(direction);
        //yield return new WaitForSeconds(20F);
    }
}
