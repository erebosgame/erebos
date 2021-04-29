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

    public void WakeUpEnd()
    {
        EarthBoss.instance.WakeUpEnd();
    }
}
