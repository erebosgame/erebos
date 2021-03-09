using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyKamikaze : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 6f;
    private bool isAggroed;
    private bool isAttacking;
    private Vector3 playerNormal;
    private Coroutine attackRoutine;
    private float lastAttack;

    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {       
        if (isAttacking)
        {
            float step =  speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, Player.gameObject.transform.position, step);
        }

        if (Vector3.Dot(playerNormal, this.transform.position - Player.gameObject.transform.position) > 0)
        {
            //add delta time
            this.rb.velocity = new Vector3(0.9f*this.rb.velocity.x, this.rb.velocity.y, 0.9f*this.rb.velocity.z);
            isAttacking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAA");
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
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && isAttacking)
        {
            Player.stats.health -= 40;
            isAttacking = false;
        }
    }
    

    IEnumerator Explode()
    {
        isAttacking = true;
        yield return new WaitForSeconds(5);

        GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
        explosion.transform.position = this.transform.position;
        UnityEngine.Object.Destroy(explosion, 10f); 
        Destroy(this.gameObject);    
        if(Vector3.Distance(Player.gameObject.transform.position,this.transform.position) < 5)
        {
            Player.stats.health -= 100;
        }
    }

    void AttackTarget(GameObject target) 
    {        
        lastAttack = Time.time;
        playerNormal = (Player.gameObject.transform.position - this.transform.position).normalized;
        
        Vector3 attackDirection = (target.transform.position - this.transform.position);
        attackDirection.y = 0;
        attackDirection = attackDirection.normalized;
        //rb.AddForce(attackDirection * attackForce);
        isAttacking = true;
    }
}
