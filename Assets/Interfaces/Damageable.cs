using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damageable
{
    int Health { get; }
    int MaxHealth { get; }
    void TakeDamage(int damage);
    void OnDeath();
}