using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBoss : MonoBehaviour
{
    public GameObject egg;
    public AirEgg eggScript;

    Animator animator;

    public static AirBoss instance;
    public int currentPhase;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPhase = 0;
        eggScript = egg.GetComponent<AirEgg>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabEgg()
    {
        egg.transform.SetParent(this.transform);
    }

    public void ReleaseEgg()
    {
        Player.stats.defeatedBosses.Add(Element.Air);
        egg.transform.SetParent(this.transform.parent);
        eggScript.onTerrain = true;
    }

    public void MoveEgg() {
        animator.SetTrigger("Move");  
        currentPhase += 1;
    }

    public void CenterEgg() 
    {
        egg.transform.localPosition = new Vector3(0f, -1.1f, -0.4f);
    }

    public void StopFlying() 
    {
        animator.SetTrigger("StopFlying");
    }

    public void Die()
    {
        gameObject.SetActive(false);
        eggScript.EnableEgg();
    }

    public static void LoadPhase(int phase)
    {
        instance.currentPhase = phase;
        instance.animator.SetInteger("Phase", phase);   
        switch (phase)
        {
            case 0:
                instance.egg.transform.position = new Vector3(1545.0f,330.5f,2370.8f);
                break;
            case 1:
                instance.egg.transform.position = new Vector3(2679.68f,281.21f,2581.96f);
                break;
            case 2:
                instance.egg.transform.position = new Vector3(1854.92f,258.14f,3302.16f);
                break;
            case 3:
                instance.egg.transform.position = new Vector3(2409.02f,278.29f,4053.15f);
                break;
            case 4:
                instance.egg.transform.position = new Vector3(3114.09f,267.11f,3374.14f);
                break;
            case 5:
                instance.egg.transform.position = new Vector3(3114.09f,267.11f,3374.14f);
                instance.Die();
                break;
        }
    }
}
