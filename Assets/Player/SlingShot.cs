using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public float force;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = transform.position;
    }

    public void Fire(Vector3 direction)
    {
        rb.AddForce(direction * 50, ForceMode.Impulse);
        
        StartCoroutine(SelfDestruct());
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Damageable>().TakeDamage(20);
            Destroy(gameObject);
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
