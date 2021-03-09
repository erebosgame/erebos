using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyKamikaze : MonoBehaviour
{
    private Rigidbody rb;
    private float speed = 5f;
    private bool isAttacking;
    private Coroutine attackRoutine;

    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {       
        if (isAttacking)
        {
            float step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Player.gameObject.transform.position, step);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && isAttacking)
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
        GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
        explosion.transform.position = this.transform.position;
        UnityEngine.Object.Destroy(explosion, 10f);
        Destroy(this.gameObject);
        if (Vector3.Distance(Player.gameObject.transform.position, this.transform.position) < 5)
        {
            Player.stats.health -= 100;
        }
    }
}
