using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorDamageable : MonoBehaviour, Damageable
{
    Meteor meteor;

    public int Health { get { return meteor.Health; } }

    public int MaxHealth { get { return meteor.MaxHealth; } }

    public void OnTriggerEnter(Collider collider) 
    {
        if (collider.CompareTag("Player"))
        {
            meteor.OnCollisionPlayer(collider);
        }        
    }

    public void OnDeath()
    {
        meteor.OnDeath();
    }

    public void TakeDamage(int damage)
    {
        meteor.TakeDamage(damage);
    }

    // Start is called before the first frame update
    void Start()
    {
        meteor = GetComponentInParent<Meteor>();
    }
    
}
