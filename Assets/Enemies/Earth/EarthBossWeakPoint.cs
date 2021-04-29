using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBossWeakPoint : MonoBehaviour, Damageable
{
    public EarthElementItem item;
    public EarthBoss earthBoss;
    public int Health { get { return earthBoss.Health; } }
    public int MaxHealth { get { return earthBoss.MaxHealth; } }

    bool active = true;
    
    public void OnDeath() 
    {
        item.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        if (!active)
            return;
        earthBoss.TakeDamage(1);
        StartCoroutine(Reactivate(10));
        active = false;

        if (earthBoss.Health <= 0)
            OnDeath();
    }

    IEnumerator Reactivate(float time)
    {
        yield return new WaitForSeconds(time);
        active = true;
    }
}
