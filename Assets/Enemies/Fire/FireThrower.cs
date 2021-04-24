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
        switch (state) {
            case State.Idle:
                if (!isMovingIdle)
                {
                    direction = Random.insideUnitCircle.normalized * Random.Range(10f,7f);
                    destination = spawnPosition + new Vector3(direction.x, 0, direction.y);
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

                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < 10F)
                {   
                    state = State.Attack;
                }
                else if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < 8F)
                {   
                    state = State.Escape;
                }
            break;
            case State.Attack:
                // isAttacking = true;
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) < 8F)
                {   
                    state = State.Escape;
                }
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) > 10F)
                {   
                    state = State.ChaseTarget;
                }
                if (Time.time > shootTimer)
                {
                    FireSling();
                    shootTimer = Time.time + 5F;
                }
                controller.LookAt(Player.gameObject.transform.position);
            break;
            case State.Escape:
                // isAttacking = false;
                controller.speed = 8;
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
                if (Vector3.Distance(this.gameObject.transform.position, Player.gameObject.transform.position) >= 10F)
                {   
                    controller.speed = 5;
                    state = State.Attack;
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
            state = State.ChaseTarget;
            // isAttacking = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
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

    void FireSling()
    {
        GameObject projectile = Instantiate(ammo, this.gameObject.transform.position, Quaternion.identity);
        Ray ray = new Ray(this.gameObject.transform.position, this.gameObject.transform.forward);
        RaycastHit hit;

        Vector3 target;
        Physics.Raycast(ray, out hit, 1000f, ~LayerMask.GetMask(), QueryTriggerInteraction.Ignore);
        if (hit.collider)
            target = hit.point;
        else
            target = ray.GetPoint(1000);

        Vector3 direction = (target - projectile.transform.position).normalized;

        projectile.transform.forward = direction;

        projectile.GetComponent<FireBall>().Fire(direction);
        //yield return new WaitForSeconds(20F);
    }
}
