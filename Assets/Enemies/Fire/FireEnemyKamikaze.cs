using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyKamikaze : MonoBehaviour
{
    private EnemyController controller;
    private Enemy enemy;

    private bool isAttacking;
    private Coroutine attackRoutine;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<EnemyController>();
        enemy = this.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {       
        if (isAttacking)
        {           
            controller.MoveTowards(Player.gameObject.transform.position, Time.deltaTime);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (attackRoutine != null)
                StopCoroutine(attackRoutine);

            attackRoutine = StartCoroutine("Explode");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Player") && isAttacking)
        {
            Explosion();
            isAttacking = false;
        }
    }

    IEnumerator Explode()
    {
        isAttacking = true;
        yield return new WaitForSeconds(10);

        Explosion();
    }

    private void Explosion()
    {
        GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        UnityEngine.Object.Destroy(explosion, 10f);
        if (Vector3.Distance(Player.gameObject.transform.position, this.transform.position) < 5)
        {
            Player.stats.TakeDamage(80);
        }
        enemy.OnDeath();
    }
}
