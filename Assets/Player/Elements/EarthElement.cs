using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class EarthElement : MonoBehaviour
{
    public GameObject explosionPrefab;

    private MeshRenderer meshRenderer;
    private Rigidbody rb;

    private float boulderRadius;

    private int hitsRemaining;

    public EarthElement(GameObject explosionPrefab)
    {
        this.explosionPrefab = explosionPrefab;
        this.meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hitsRemaining = 3;

        Player.gameObject.GetComponent<CharacterController>().enabled = false;
        Player.stats.elementState = Element.Earth;
        
        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        Player.gameObject.transform.localPosition = Vector3.zero;

        rb.velocity = Player.movement.currentVelocity;

        StartCoroutine("EndAfterTime");
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 relativeMoveDirection = Vector3.zero;
        if (moveDirection.magnitude > 0)
        {
            relativeMoveDirection = GetRelativeDirection(moveDirection);
        }
        this.rb.AddForce(relativeMoveDirection*8000f*Time.deltaTime);
        if (this.rb.velocity.magnitude >= 40)
            this.rb.velocity = this.rb.velocity.normalized * 40;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 direction = (collision.collider.transform.position - Player.gameObject.transform.position).normalized;
            
        if (collision.collider.tag == "Enemy")
        {
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            explosion.transform.position = collision.contacts[0].point;
            UnityEngine.Object.Destroy(explosion, 10f);
            collision.collider.GetComponent<Rigidbody>().AddForce(100f*direction);
            rb.AddForce(-500f*direction*rb.velocity.magnitude);
            collision.collider.gameObject.GetComponent<Enemy>().TakeDamage(rb.velocity.magnitude/40f * 40);

            hitsRemaining--;
            if (hitsRemaining <= 0)
            {
                End();
            }
        }
    }

    IEnumerator EndAfterTime()
    {
        yield return new WaitForSeconds(30f);
        End();
    }
    public void End() 
    {
        transform.DetachChildren();
        Player.gameObject.GetComponent<CharacterController>().enabled = true;
        Player.gameObject.transform.rotation = Quaternion.identity;
        Player.stats.elementState = Element.NoElement;
        meshRenderer.enabled = true;
        UnityEngine.Object.Destroy(this.gameObject);
    }
    public Vector3 GetRelativeDirection(Vector3 direction) 
    {
        return Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * direction;
    }
}