using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float health;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float ammount)
    {
        health -= ammount;

        if(health <= 0)
            Die();  
    }

    private void Die()
    {
        //droploot
        //play animation  
        Player.stats.AddExperience(5);   
        Destroy(this.gameObject);
    }
}
