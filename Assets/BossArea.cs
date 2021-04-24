using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossArea : MonoBehaviour
{
    public GameObject boss;
    Damageable bossDamageable;

    private void Start() {
        bossDamageable = boss.GetComponent<Damageable>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && bossDamageable.Health > 0)
        {
            BossUI.SetActive(true);
        }    
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            BossUI.SetActive(false);
        }    
    }

    private void OnTriggerStay(Collider other) {
        if (bossDamageable.Health > 0)
        {
            BossUI.UpdateHealth((float) bossDamageable.Health / bossDamageable.MaxHealth);
        }
        else
        {
            BossUI.SetActive(false);
        }
    }
}
