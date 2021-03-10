using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Targetable, Damageable
{
    public GameObject healthBarPrefab;
    private HpBar healthBar;
    private Outline outline;
    public int maxHealth;
    public int health;

    private Spawner spawner;

    public void OnTargetStart() 
    { 
        if (outline)
            outline.OutlineWidth = 2; 
    }
    public void OnTargetStop() 
    { 
        if (outline)
            outline.OutlineWidth = 0; 
    }

    void Start()
    {
        if (healthBarPrefab)
        {
            GameObject hpObj = Instantiate(healthBarPrefab, this.transform);
            hpObj.transform.localPosition = new Vector3(0,1.5f,0);
            healthBar = hpObj.GetComponent<HpBar>();
        }
        
        outline = GetComponent<Outline>();
        if(outline)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 0;
        }
        
        maxHealth = 100;
        health = maxHealth;

        if (this.transform.parent && this.transform.parent.CompareTag("Spawner"))
        {
            spawner = this.transform.parent.GetComponent<Spawner>();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
            OnDeath();  

        healthBar.UpdateHealth(health, maxHealth);
    }

    public void OnDeath()
    {
        Player.stats.AddExperience(5);

        if (spawner)
            spawner.OnSpawnedDeath();

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            TakeDamage(10);
        }
    }
}
