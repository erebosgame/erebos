using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBossWeakPoint : MonoBehaviour, Damageable
{
    public EarthEnemyBoss earthBoss;
    public int Health { get { return earthBoss.Health; } }
    public int MaxHealth { get { return earthBoss.MaxHealth; } }

    bool active = true;
    
    public void OnDeath() {}
    public void TakeDamage(int damage)
    {
        earthBoss.TakeDamage(1);
        active = false;
        StartCoroutine(Reactivate(10));
    }

    IEnumerator Reactivate(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
