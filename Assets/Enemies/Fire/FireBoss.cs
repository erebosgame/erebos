using System.Collections;
using UnityEngine;
using Cinemachine;

enum FireBossState {
    Sleeping,
    Waiting,
    CannonPreparing,
    MeteorShot,
    Recalling,
    Dead
}

public class FireBoss : MonoBehaviour, Damageable
{
    CinemachineImpulseSource impulse;
    public GameObject platforms;
    public GameObject fireElement;
    public GameObject doors;
    public GameObject mouth;
    public GameObject cannon;  
    public Meteor projectile;
    private Animator animator;
    private SphereCollider triggerArea;

    int health;
    int maxHealth;
    public int Health { get { return health; } }
    public int MaxHealth { get { return maxHealth; } }
    int attackCost;

    FireBossState bossState = FireBossState.Sleeping;
    float recallTime = 10f;
    float reloadTime = 5f;

    public static FireBoss instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
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
    }

    private IEnumerator AILoop() {
        while (health > 0)
        {
            print(bossState);
            switch (bossState)
            {
                case FireBossState.Waiting:
                    Shoot();
                    yield return new WaitForSeconds(recallTime);
                    break;
                case FireBossState.MeteorShot:
                    Recall();
                    yield return new WaitForSeconds(reloadTime);
                    break;
                case FireBossState.Sleeping:
                    yield break;
                default:
                    yield return new WaitForSeconds(1f);
                    break;
            }
        }
    }

    public void Shoot()
    {
        animator.SetTrigger("shoot");
        bossState = FireBossState.CannonPreparing;
    }

    public void _FireMeteor()
    {
        UpdateHealth(health - attackCost);
        Vector3 direction = (Player.gameObject.transform.position - cannon.gameObject.transform.position).normalized;
        projectile.Fire(cannon.transform.position, direction);
        bossState = FireBossState.MeteorShot;
    }

    public void Recall()
    {
        animator.SetTrigger("eat");
        bossState = FireBossState.Recalling;
    }

    public void _Recall()
    {
        projectile.Recall(mouth);
    }

    public void Eat()
    {
        UpdateHealth(health + attackCost);
        RecallEnd();
    }

    public void RecallEnd()
    {
        animator.SetTrigger("eat_end");
        bossState = FireBossState.Waiting;
    }

    public void WakeUp() 
    {
        animator.SetTrigger("enterarea");
        bossState = FireBossState.Waiting;
        StartCoroutine("AILoop");
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && bossState == FireBossState.Sleeping)
        {  
            WakeUp();
        }
    }

    private void UpdateHealth(int health)
    {
        this.health = health;
    }

    public void OnMeteorDestroy()
    {
        if (bossState == FireBossState.Recalling)
        {
            RecallEnd();
        }
        else if (bossState == FireBossState.MeteorShot)
        {
            bossState = FireBossState.Waiting;
        }
        if (health == 1) 
        {
            OnDeath();
        }
    }

    IEnumerator Sink() {
        int steps = 300;
        float total_transform_boss = -160f;
        float total_transform_platf = 21f;
        float total_transform_doors = -210f;
        float total_time = 5f;
        for (int i = 0; i < steps; i++)
        {
            impulse.GenerateImpulse();
            transform.position = new Vector3(transform.position.x, 
                                                transform.position.y+(1.0f/steps)*total_transform_boss, 
                                                transform.position.z);
            platforms.transform.position = new Vector3(platforms.transform.position.x, 
                                                        platforms.transform.position.y+(1.0f/steps)*total_transform_platf, 
                                                        platforms.transform.position.z);
            doors.transform.position = new Vector3(doors.transform.position.x, 
                                                    doors.transform.position.y+(1.0f/steps)*total_transform_doors, 
                                                    doors.transform.position.z);
            yield return new WaitForSeconds(total_time/steps);
            print(total_time/steps);
        }
        fireElement.transform.SetParent(this.transform.parent);
        this.gameObject.SetActive(false);        
    }

    public static void LoadKill(float angle)
    {
        instance.UpdateHealth(0);
        Player.stats.defeatedBosses.Add(Element.Fire);
        // instance.animator.SetTrigger("skip");
        instance.transform.position = new Vector3(instance.transform.position.x, 
                                                    instance.transform.position.y - 180f, 
                                                    instance.transform.position.z);
        instance.platforms.transform.position = new Vector3(instance.platforms.transform.position.x, 
                                                            instance.platforms.transform.position.y + 21f, 
                                                            instance.platforms.transform.position.z);
        instance.doors.transform.position = new Vector3(instance.doors.transform.position.x, 
                                                        instance.doors.transform.position.y - 210f, 
                                                        instance.doors.transform.position.z);
        print(PlayerPrefs.GetFloat("FireBossDeathAngle"));
        instance.transform.parent.transform.localEulerAngles = new Vector3(0, angle, 0);
        instance.fireElement.transform.SetParent(instance.transform.parent);
        instance.fireElement.transform.localPosition = new Vector3(-1f, 23f, 55f);
        instance.fireElement.transform.localRotation = Quaternion.Euler(180,0,0);
        instance.gameObject.SetActive(false);
    }

    public static void Reset() 
    {
        if (instance.bossState != FireBossState.Sleeping && instance.bossState != FireBossState.Dead)
        {
            instance.bossState = FireBossState.Sleeping;
            instance.animator.SetTrigger("reset");
            instance.UpdateHealth(instance.maxHealth);
            instance.projectile.gameObject.SetActive(false);
        }
    }

    public void TakeDamage(int damage) {}

    public void OnDeath()
    {
        bossState = FireBossState.Dead;
        UpdateHealth(0);
        StartCoroutine("Sink");
        Player.stats.defeatedBosses.Add(Element.Fire);    
    }

    /*  public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        Debug.Log(currentTime);
        return currentTime;
    } */

}
