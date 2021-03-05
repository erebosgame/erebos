using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 100;
    [Header("HealthBar")]
    public Image HpBar;
    // Start is called before the first frame update
    void Start()
    {
        HpBar.fillAmount = health / 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10f);
        }

        if(HpBar.fillAmount == 1)
        {
            HpBar.enabled = false;
        }
    }

    public void TakeDamage(float ammount)
    {
        HpBar.enabled = true;

        health -= ammount;

        HpBar.fillAmount = health / 100f;

        if(health <= 0)
            Die();  
    }

    private void Die()
    {
        //droploot
        //play animation  
        Destroy(this.gameObject);
    }
}
