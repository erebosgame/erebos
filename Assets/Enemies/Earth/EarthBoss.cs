using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

enum EarthBossState {
    Sleeping, 
    ArmUp,
    ArmDown,
    Dead
}
public class EarthBoss : MonoBehaviour, Damageable
{
    bool isCrystalDown;
    public static EarthBoss instance;
    bool playerStanding;
    EarthBossState bossState;
    public bool attack;
    private bool done;
    public MeshCollider[] colliders;
    public Dictionary<Collider, Vector3> colliderVectors;
    public GameObject fistTarget;
    public GameObject elbowTarget;
    public GameObject resetTarget;
    public BioIK.BioIK bioIK;

    public WaveAttack waveAttack;

    private Dictionary<string, BioIK.BioSegment> segments;
    private Vector3 fistPosition = new Vector3(2.39f,1.75f,2.22f);
    BioIK.Position elbowObjective;
    Vector3 direction;
    Vector3 target;
    Animator animator;

    public GameObject[] crystals;
    List<GameObject> crystalsLeft; 

    int currentCrystal;
    CinemachineImpulseSource impulse;
    int health;
    int maxHealth;

    public int Health { get { return health; } }

    public int MaxHealth { get { return maxHealth; } }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        bossState = EarthBossState.Sleeping;
        impulse = GetComponent<CinemachineImpulseSource>();
        animator = GetComponent<Animator>();
        maxHealth = 5;
        health = maxHealth;
        colliderVectors = new Dictionary<Collider, Vector3>();
        elbowObjective = (BioIK.Position) bioIK.Segments.Where(s => s.name.Equals("LowerArm.R")).Single().Objectives.GetValue(0);
        colliders = GetComponentsInChildren<MeshCollider>();
        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
        segments = new Dictionary<string, BioIK.BioSegment>();
        bioIK.Segments.ForEach(s => segments[s.name] = s);
    }

    public IEnumerator AILoop() {
        while (health > 0)
        {
            switch (bossState)
            {
                case EarthBossState.ArmUp:
                    if (isCrystalDown)
                    {
                        RightAttack();
                        yield return new WaitForSeconds(12f);
                    }
                    else
                    {
                        LeftAttack();
                        yield return new WaitForSeconds(10f);
                    }
                    break;
                case EarthBossState.ArmDown:
                    RetractArm();
                    yield return new WaitForSeconds(4f);
                    break;
                case EarthBossState.Sleeping:
                    yield return new WaitForSeconds(5f);
                    yield break;
                case EarthBossState.Dead:
                    yield break;
            }
        }
    }

    void RightAttack() 
    {
        RaycastHit hit;
        bossState = EarthBossState.ArmDown;
        Physics.Raycast(Player.gameObject.transform.position, Vector3.down, out hit);
        target = hit.point + Vector3.down * 15f;            
        // elbowObjective.enabled = false;
    }

    void LeftAttack()
    {
        animator.SetTrigger("thomp");
        // StartCoroutine(ActivateBioIK(150,0.5f,false));
    }

    void RetractArm()
    {
        bossState = EarthBossState.ArmUp;
        fistTarget.transform.localPosition = fistPosition;
        elbowObjective.enabled = true;
    }

    public static void LoadKill()
    {        
        instance.bossState = EarthBossState.Dead;
        instance.animator.SetTrigger("loadkill");
        Player.stats.defeatedBosses.Add(Element.Earth);   
    }
    public static void Reset() 
    {
        if (instance.bossState != EarthBossState.Sleeping && instance.bossState != EarthBossState.Dead)
        {
            instance.bossState = EarthBossState.Sleeping;
            instance.animator.SetTrigger("reset");
            instance.health = instance.maxHealth;
            foreach (GameObject crystal in instance.crystals)
            {
                crystal.transform.position = new Vector3(crystal.transform.position.x, 0, crystal.transform.position.z);
                crystal.SetActive(false);
            }
            instance.isCrystalDown = false;
            instance.currentCrystal = 0;
            instance.StartCoroutine(instance.ActivateBioIK(1,0.1f,false));
        }
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.Log(colliders);
        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
        
        Physics.Raycast(Player.gameObject.transform.position, Vector3.down, out hit, 2f, LayerMask.GetMask("Terrain"), QueryTriggerInteraction.Ignore);
        if (hit.collider)
        {
            if (colliders.Contains(hit.collider))
            {
                Player.ActiveGameObject.transform.SetParent(hit.collider.transform.parent);
                playerStanding = true;
            }
            else
            {
                Player.ActiveGameObject.transform.SetParent(null);
                playerStanding = false;
            }
        }
        if (bossState == EarthBossState.ArmDown)        
            MoveFistToTarget();
    }

    public void StartWaveAttack()
    {
        StartCoroutine("LiftCrystal");
        isCrystalDown = true;

        impulse.GenerateImpulse();

        waveAttack.gameObject.transform.localScale = new Vector3(1,1,1);
        waveAttack.gameObject.SetActive(true);
        // StartCoroutine(ActivateBioIK(150,0.5f,true));
    }

    private void MoveFistToTarget()
    {
        fistTarget.transform.position = Vector3.MoveTowards(fistTarget.transform.position, target, 350f*Time.deltaTime );
        Vector3 newDirection = (fistTarget.transform.position - target);
        if (newDirection.magnitude < 0.1f)
        {
            newDirection = direction;
        }
        else
        {
            direction = newDirection.normalized;
        }
    }

    public void WakeUpEnd()
    {
        if (bossState == EarthBossState.Sleeping)
        {
            bossState = EarthBossState.ArmUp;
            StartCoroutine(ActivateBioIK(400,3f,true));
            StartCoroutine(AILoop());
        }
    }

    public IEnumerator LiftCrystal()
    {
        int index = currentCrystal;
        crystals[index].SetActive(true);
        int steps = 200;
        float total_time = 3f;
        float total_distance = 0.43f;
        for (int i = 0; i <= steps; i++)
        {
            yield return new WaitForSeconds(total_time/steps);
            crystals[index].transform.position = new Vector3(crystals[index].transform.position.x, 
                                                                crystals[index].transform.position.y + (float)i/steps*total_distance, 
                                                                crystals[index].transform.position.z);
            
            //print(bioik.AnimationBlend);
        }
        crystals[index].GetComponent<SphereCollider>().enabled = true;
        currentCrystal += 1;
    }

    public IEnumerator ActivateBioIK(int steps, float total_time, bool activate)
    {
        for (int i = 0; i <= steps; i++)
        {
            yield return new WaitForSeconds(total_time/steps);
            bioIK.AnimationBlend = 1 - (float)i / steps;
            bioIK.AnimationWeight = 1 - (float)i / steps;
            if (!activate)
            {
                bioIK.AnimationBlend = 1 - bioIK.AnimationBlend;
                bioIK.AnimationWeight = 1 - bioIK.AnimationWeight;   
            }
            //print(bioik.AnimationBlend);
        }
    }

    private bool IsIdle()
    {
        return colliders.All(c => GetVelocity(c) < 0.1f);
    }

    private float GetVelocity(Collider c)
    {
        return (c.transform.position - colliderVectors[c]).magnitude;
    }   

    public void OnChildTriggerEnter(Collider collided, Collider collider)
    {
        // print("test " + collider.name);
        if (collider.CompareTag("Player"))
        {
            float v = GetVelocity(collided);
            // print("hey");

            if (v > 1f && !playerStanding && bossState == EarthBossState.ArmDown) 
            {
                Vector3 direction = (collider.transform.position - collided.transform.position).normalized;
                direction.y = 0;
                direction = Quaternion.AngleAxis(70,Vector3.Cross(direction, Vector3.up)) * direction;
                direction = direction.normalized;
                if (v*10 >= 50f)
                    Player.stats.TakeDamage(50);
                else
                    Player.stats.TakeDamage((int) (v*10));
                Player.movement.PushPlayer(direction, v*5f);   
            }
        }
        
        if (collider.CompareTag("Crystal") && collider.isTrigger)
        {
            isCrystalDown = false;
            collider.gameObject.SetActive(false);
            TakeDamage(1);
        }

    }

    public void OnChildTriggerExit(Collider collided, Collider collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (Player.ActiveGameObject.transform.parent == collider.transform.parent)
        {
            Player.ActiveGameObject.transform.SetParent(null);
        }
    }

    public void TakeDamage(int damage)
    {
        // Vector3 direction = (resetTarget.transform.position - Player.gameObject.transform.position).normalized;
        // direction.y = 1f;
        // direction = direction.normalized;
        // Player.movement.PushPlayer(direction, 50f, 10f);   
        health -= 1;
        if (health <= 0)
            OnDeath();
    }

    public void OnDeath()
    {            
        Player.stats.defeatedBosses.Add(Element.Earth);   
        bossState = EarthBossState.Dead;
        StartCoroutine(ActivateBioIK(150,0.5f,false));

        animator.SetTrigger("death");
    }
}

