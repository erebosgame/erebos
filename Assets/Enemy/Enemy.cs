using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject healthBarPrefab;
    private GameObject healthBar;
    private Outline outline;
    public float maxHealth = 100;
    public float health;
    

    void Awake()
    {
        healthBar = Instantiate(healthBarPrefab, this.gameObject.transform);
        healthBar.transform.localPosition = new Vector3(0,1.5f,0);
        outline = this.gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 0;
    }

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
