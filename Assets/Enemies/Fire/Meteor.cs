 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour, Damageable
{
    public FireBoss fireBoss;
    public GameObject healthBarPrefab;
    HpBar healthBar;
    int hitMax;
    int hit;
    bool attacking;

    GameObject recallTarget;
    Collider recallCollider;
    Rigidbody rb;
    public GameObject destroyParticles;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject healthBarContainer = Instantiate(new GameObject("healthBarContainer"), this.transform);
        healthBarContainer.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        GameObject hpObj = Instantiate(healthBarPrefab, healthBarContainer.transform);
        hpObj.transform.localPosition = new Vector3(0, 1.5f, 0);
        healthBar = hpObj.GetComponent<HpBar>();
        healthBar.size = 0.15f;
        hitMax = 10;
    }

    public void Fire(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.forward = direction;
        gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        gameObject.SetActive(true);
        rb.isKinematic = false; 
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * 100, ForceMode.Impulse);
        hit = hitMax; 
        healthBar.UpdateHealth(hit, hitMax);   
        attacking = true;    
    }

    public void Recall(GameObject target)
    {
        gameObject.layer = LayerMask.NameToLayer("BossEatable");
        recallTarget = target;
        recallCollider = target.GetComponent<Collider>();
        Vector3 direction = (target.transform.position - transform.position).normalized;
        rb.isKinematic = false; 
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * 180, ForceMode.Impulse);
    }
    public void OnTriggerEnter(Collider collider) 
    {
        if (collider == recallCollider) 
        {
            fireBoss.Eat();
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage)
    {
        hit -= 1;

        if (hit <= 0)
            OnDeath();

        healthBar.UpdateHealth(hit, hitMax);
    }

    public void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        healthBar.UpdateHealth(hit, hitMax, true);  
        attacking = false;
    }

    public void OnCollisionPlayer(Collider collider)
    {
        if (attacking) {
            Player.stats.health -= 50;
        }
    }

    public void OnDeath()
    {
        Instantiate(destroyParticles, this.transform.position, Quaternion.identity);
        
        fireBoss.OnMeteorDestroy();
        this.gameObject.SetActive(false);
    }
}
