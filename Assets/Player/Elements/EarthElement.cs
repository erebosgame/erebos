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

    public EarthElement(GameObject boulderPrefab, GameObject explosionPrefab)
    {
        this.explosionPrefab = explosionPrefab;
        this.meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Player.gameObject.GetComponent<CharacterController>().detectCollisions = false;
        Player.stats.elementState = Element.Fire;
        
        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        Player.gameObject.transform.localPosition = Vector3.zero;

        rb.velocity = Player.movement.currentVelocity;
    }

    void Update()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 relativeMoveDirection = Vector3.zero;
        if (moveDirection.magnitude > 0)
        {
            relativeMoveDirection = GetRelativeDirection(moveDirection);
        }
        this.rb.AddForce(relativeMoveDirection*7000f*Time.deltaTime);
        if (this.rb.velocity.magnitude >= 30)
            this.rb.velocity = this.rb.velocity.normalized * 30;
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 direction = (collision.collider.transform.position - Player.gameObject.transform.position).normalized;
            
        if (collision.collider.tag == "Enemy")
        {
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            explosion.transform.position = collision.contacts[0].point;
            UnityEngine.Object.Destroy(explosion, 10f);
            collision.collider.GetComponent<Rigidbody>().AddForce(500f*direction);
            rb.AddForce(-500f*direction*rb.velocity.magnitude);
            // End();
        }
    }

    public void End() 
    {
        meshRenderer.enabled = true;
        Player.stats.elementState = Element.NoElement;
        transform.DetachChildren();
        UnityEngine.Object.Destroy(this.gameObject);
    }
    public Vector3 GetRelativeDirection(Vector3 direction) 
    {
        return Quaternion.LookRotation(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * direction;
    }
}