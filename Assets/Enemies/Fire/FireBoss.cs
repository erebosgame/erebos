using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBoss : MonoBehaviour
{
    public GameObject platforms;
    public GameObject fireElement;
    public Image hpBar;
    public GameObject mouth;
    public GameObject cannon;  
    public Meteor projectile;
    private Animator animator;
    private SphereCollider triggerArea;

    int health;
    int maxHealth;

    int attackCost;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        triggerArea = GetComponent<SphereCollider>();
        attackCost = 20;
        maxHealth = 101;
        health = maxHealth;
    }

    public void Update()
    {
        if (health > 0)
        {
            Vector3 direction = (Player.gameObject.transform.position - transform.parent.transform.position);
            direction.y = 0;
            direction = direction.normalized;
            transform.parent.transform.forward = direction;
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            animator.SetTrigger("eat");
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            animator.SetTrigger("shoot");
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            Die();
        }
    }

    public void Recall()
    {
        projectile.Recall(mouth);
    }

    public void Eat()
    {
        UpdateHealth(health + attackCost);
        animator.SetTrigger("eat_end");
    }

    public void Shoot()
    {
        UpdateHealth(health - attackCost);
        Vector3 direction = (Player.gameObject.transform.position - cannon.gameObject.transform.position).normalized;
        projectile.Fire(cannon.transform.position, direction);

        // projectile.GetComponent<FireBall>().Fire(direction);
        //yield return new WaitForSeconds(20F);
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            animator.SetTrigger("enterarea");
        }
    }

    private void UpdateHealth(int health)
    {
        this.health = health;
        hpBar.fillAmount = (float) health / maxHealth;
    }

    public void OnMeteorDestroy()
    {
        if (health == 1) {
            Die();
        }
    }

    private void Die()
    {
        UpdateHealth(0);
        StartCoroutine("Sink");
    }

    IEnumerator Sink() {
        int steps = 300;
        float total_transform_boss = 160f;
        float total_transform_platf = 21f;
        float total_time = 5f;
        for (int i = 0; i < steps; i++)
        {
            transform.position = new Vector3(transform.position.x, 
                                                transform.position.y-(1.0f/steps)*total_transform_boss, 
                                                transform.position.z);
            platforms.transform.position = new Vector3(platforms.transform.position.x, 
                                                        platforms.transform.position.y+(1.0f/steps)*total_transform_platf, 
                                                        platforms.transform.position.z);
            yield return new WaitForSeconds(total_time/steps);
            print(total_time/steps);
        }
        fireElement.transform.SetParent(this.transform.parent);
        this.gameObject.SetActive(false);
    }

    /*  public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        Debug.Log(currentTime);
        return currentTime;
    } */

}
