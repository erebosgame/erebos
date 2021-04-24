using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EarthEnemyBoss : MonoBehaviour
{
    public bool attack;
    private bool done;
    public MeshCollider[] colliders;
    public Dictionary<Collider, Vector3> colliderVectors;
    public GameObject fistTarget;
    public GameObject elbowTarget;
    public BioIK.BioIK bioIK;

    private Dictionary<string, BioIK.BioSegment> segments;
    private Vector3 fistPosition = new Vector3(1.5f, 2.1f, 1.7f);
    BioIK.Position elbowObjective;
    Vector3 direction;
    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        elbowTarget.transform.position = fistTarget.transform.position + direction * 100;
        colliderVectors = new Dictionary<Collider, Vector3>();
        elbowObjective = (BioIK.Position) bioIK.Segments.Where(s => s.name.Equals("LowerArm.R")).Single().Objectives.GetValue(0);
        colliders = GetComponentsInChildren<MeshCollider>();
        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
        segments = new Dictionary<string, BioIK.BioSegment>();
        bioIK.Segments.ForEach(s => segments[s.name] = s);
        //print(segments["Head 1"].Joint.X.LowerLimit);
        WakeUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            attack = !attack;
            if (attack)
            {
                target = Player.gameObject.transform.position + Vector3.down * 15f;            
                ToConvex();
            }
            else
            {
                fistTarget.transform.localPosition = fistPosition;
                elbowObjective.enabled = true;
                ToConcave();
            }
        }

        if (attack)
        {
            elbowObjective.enabled = false;
            MoveFistToTarget();
        }

        if (IsIdle())
        {                
            ToConcave();
        }        
        colliders.ToList().ForEach(c => colliderVectors[c] = c.transform.position);
    }

    public void WakeUp()
    {
        segments["Head 1"].Joint.X.UpperLimit = 20;
        segments["Head 1"].Joint.X.TargetValue = 0;
        segments["Torso"].Joint.X.UpperLimit = 10;
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

        elbowTarget.transform.position = fistTarget.transform.position + direction * 100;
    }

    private void ToConvex()
    {
        // colliders.ToList().ForEach(c => c.convex = true);
    }
    private void ToConcave()
    {
        // colliders.ToList().Where(c => !c.isTrigger).ToList().ForEach(c => c.convex = false);
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
        else
        {
            Player.ActiveGameObject.transform.SetParent(collided.transform.parent);
            Debug.Log("Player" + Player.gameObject.transform.forward);
            Debug.Log("Camera" + Camera.main.transform.forward);
            Debug.Log("Direzione palla" + collider.transform.forward);
            print(collider.name + " enter");
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
}

