﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnemyRock : MonoBehaviour
{
    private Rigidbody rb;
    private float attackForce;
    private bool isAggroed;
    private bool isAttacking;
    private Vector3 playerNormal;
    private Coroutine attackRoutine;
    private float lastAttack;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        attackForce = 1200f;
    }

    // Update is called once per frame
    void Update()
    {       
        if (isAggroed)
        {
            if (isAttacking)
            {
                Vector3 newRotation = new Vector3(Player.gameObject.transform.position.x - this.transform.position.x, 0 ,Player.gameObject.transform.position.z - this.transform.position.z);
                rb.velocity = Vector3.RotateTowards(rb.velocity, newRotation.normalized * rb.velocity.magnitude, 0.8f* Time.deltaTime, 1f);
            }

            if (!isAttacking || Vector3.Dot(playerNormal, this.transform.position - Player.gameObject.transform.position) > 0)
            {
                //add delta time
                this.rb.velocity = new Vector3(0.9f*this.rb.velocity.x, this.rb.velocity.y, 0.9f*this.rb.velocity.z);
                isAttacking = false;
            }
        }
        else
        {                
            this.rb.velocity = new Vector3(0.3f*this.rb.velocity.x, this.rb.velocity.y, 0.3f*this.rb.velocity.z);
            this.rb.angularVelocity = new Vector3(0.3f*this.rb.angularVelocity.x, 0.3f*this.rb.angularVelocity.y, 0.3f*this.rb.angularVelocity.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attackRoutine != null)
                StopCoroutine(attackRoutine);

            attackRoutine = StartCoroutine("Attack");
            isAggroed = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            isAggroed = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && isAttacking)
        {
            Player.stats.TakeDamage(40);
            isAttacking = false;
        }
    }
    

    IEnumerator Attack()
    {
        if (lastAttack != 0 && Time.time-lastAttack < 3)
            yield return new WaitForSeconds(3-(Time.time-lastAttack));
        
        while(true) 
        {
            AttackTarget(Player.gameObject);
            yield return new WaitForSeconds(3);
        }
    }

    void AttackTarget(GameObject target) 
    {        
        lastAttack = Time.time;
        playerNormal = (Player.gameObject.transform.position - this.transform.position).normalized;
        
        Vector3 attackDirection = (target.transform.position - this.transform.position);
        attackDirection.y = 0;
        attackDirection = attackDirection.normalized;
        rb.AddForce(attackDirection * attackForce);
        isAttacking = true;
    }
}
