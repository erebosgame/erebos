using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBossWeakPoint : MonoBehaviour, Damageable
{
    public EarthEnemyBoss earthBoss;

    public int Health { get { return earthBoss.Health; } }

    public int MaxHealth { get { return earthBoss.MaxHealth; } }

    public void OnDeath() {}
    public void TakeDamage(int damage)
    {
        earthBoss.TakeDamage(1);
    }
}
