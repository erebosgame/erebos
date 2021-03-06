using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonk : MonoBehaviour
{
    public static Bonk instance;
    
    private Animator animator;
    
    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformAttack()
    {
        animator.SetTrigger("BaseAttack");
    }

    public void OnAttackEnd()
    {
        Player.stats.isAttacking = false;
    }
}
