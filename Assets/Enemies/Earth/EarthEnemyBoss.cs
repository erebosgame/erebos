using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarthEnemyBoss : MonoBehaviour, Damageable
{
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

    int health;
    int maxHealth;

    public int Health { get { return health; } }

    public int MaxHealth { get { return maxHealth; } }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = 3;
        health = maxHealth;
        colliderVectors = new Dictionary<Collider, Vector3>();
        elbowObjective = (BioIK.Position) bioIK.Segments.Where(s => s.name.Equals("LowerArm.R")).Single().Objectives.GetValue(0);
        colliders = GetComponentsInChildren<MeshCollider>();
        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
        segments = new Dictionary<string, BioIK.BioSegment>();
        bioIK.Segments.ForEach(s => segments[s.name] = s);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.P))
        {
            attack = !attack;
            if (attack)
            {
                Physics.Raycast(Player.gameObject.transform.position, Vector3.down, out hit);
                target = hit.point + Vector3.down * 15f;            
            }
            else
            {
                fistTarget.transform.localPosition = fistPosition;
                elbowObjective.enabled = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetTrigger("thomp");
            StartCoroutine(ActivateBioIK(150,0.5f,false));
        }

        if (attack)
        {
            elbowObjective.enabled = false;
            MoveFistToTarget();
        }

        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
        
        Physics.Raycast(Player.gameObject.transform.position, Vector3.down, out hit, 2f);
        if (hit.collider)
        {
            if (colliders.Contains(hit.collider))
            {
                Player.ActiveGameObject.transform.SetParent(hit.collider.transform.parent);
            }
            else
            {
                Player.ActiveGameObject.transform.SetParent(null);
            }
        }
    }

    public void StartWaveAttack()
    {
        waveAttack.gameObject.transform.localScale = new Vector3(1,1,1);
        waveAttack.gameObject.SetActive(true);
        StartCoroutine(ActivateBioIK(150,0.5f,true));
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
        if (!collider.CompareTag("Player"))
            return;

        float v = GetVelocity(collided);
        // print("hey");

        if (v > 1f) 
        {
            print("wow");
            Vector3 direction = (collider.transform.position - collided.transform.position).normalized;
            direction.y = 0;
            direction = Quaternion.AngleAxis(70,Vector3.Cross(direction, Vector3.up)) * direction;
            direction = direction.normalized;
            print("Damage: "+ v);
            Player.stats.TakeDamage((int) v);
            Player.movement.PushPlayer(direction, 10f);   
        }
    }

    public void OnChildTriggerExit(Collider collided, Collider collider)
    {
        if (!collider.CompareTag("Player"))
            return;

        if (Player.ActiveGameObject.transform.parent == collider.transform.parent)
        {
            Player.ActiveGameObject.transform.SetParent(null);
            print(collider.name + " exit");
        }
    }

    public void TakeDamage(int damage)
    {
        Vector3 direction = (resetTarget.transform.position - Player.gameObject.transform.position).normalized;
        direction.y = 1f;
        direction = direction.normalized;
        Player.movement.PushPlayer(direction, 50f);   
        health -= 1;
        if (health <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        animator.SetTrigger("death");
    }
}

