using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnemyRock : MonoBehaviour
{
    private Rigidbody rb;
    private float attackForce;

    private bool isAggroed;
    private bool isAttacking;

    Vector3 attackOrigin;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        attackForce = 1000f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 normal = (Player.gameObject.transform.position - attackOrigin).normalized;
        if (Vector3.Dot(normal, this.transform.position - Player.gameObject.transform.position) > 0)
        {
            this.rb.velocity = new Vector3(0.85f*this.rb.velocity.x, this.rb.velocity.y, 0.85f*this.rb.velocity.z);
            isAttacking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isAggroed = true;
            StartCoroutine("Attack");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isAggroed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player" && isAttacking)
        {
            Player.stats.health -= 40;
            isAttacking = false;
        }
    }
    

    IEnumerator Attack()
    {
        while(isAggroed) 
        {
            AttackTarget(Player.gameObject);
            yield return new WaitForSeconds(3);
        }
    }

    void AttackTarget(GameObject target) 
    {        
        attackOrigin = this.transform.position;
        Vector3 attackDirection = (target.transform.position - this.transform.position);
        attackDirection.y = 0;
        attackDirection = attackDirection.normalized;
        rb.AddForce(attackDirection * attackForce);
        isAttacking = true;
    }
}
