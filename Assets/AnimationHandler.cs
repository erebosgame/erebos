using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    public SphereCollider triggerArea;

    public BioIK.BioIK bioik;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bioik = GetComponentInChildren<BioIK.BioIK>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            animator.SetTrigger("areaenter");
        }
    }

  /*  public float GetCurrentAnimatorTime(Animator targetAnim, int layer = 0)
    {
        AnimatorStateInfo animState = targetAnim.GetCurrentAnimatorStateInfo(layer);
        float currentTime = animState.normalizedTime % 1;
        Debug.Log(currentTime);
        return currentTime;
    } */

    public void wakeUpEnd()
    {
        StartCoroutine("ActivateBioIK");
    }

    IEnumerator ActivateBioIK()
    {
        int steps = 20;
        float min = 0f;
        float total_time = 1f;
        for (int i = 0; i < 20; i++)
        {
            yield return new WaitForSeconds(total_time/steps);
            bioik.AnimationBlend = 1 - (float)i*(1-min) / steps;
            bioik.AnimationWeight = 1 - (float)i*(1-min) / steps;
            //print(bioik.AnimationBlend);
        }
    }
}
