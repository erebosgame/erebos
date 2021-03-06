using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStaticDamageble : MonoBehaviour, Damageable
{
    int maxHit;
    int hit;
    private HpBar healthBar;
    public GameObject healthBarPrefab;

    public int Health { get { return hit; } }
    public int MaxHealth { get { return maxHit; } }


    // Start is called before the first frame update
    void Start()
    {
        maxHit = 5;
        hit = maxHit;
        if (healthBarPrefab)
        {
            GameObject hpObj = Instantiate(healthBarPrefab, this.transform);
            hpObj.transform.localPosition = new Vector3(0, 1.5f, 0);
            healthBar = hpObj.GetComponent<HpBar>();
        }
    }
 
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDeath()
    {
        //stop render
        //start respawner
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("hit");
        hit -= 1;

        if (hit <= 0)
            OnDeath();

        healthBar.UpdateHealth(hit, 5);
    }
}
