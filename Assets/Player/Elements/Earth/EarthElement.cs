using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class EarthElement : MonoBehaviour
{
    Camera mainCamera;

    public GameObject explosionPrefab;

    private Rigidbody rb;

    private float boulderRadius;

    private int hitsRemaining;

    public EarthElement(GameObject explosionPrefab)
    {
        this.explosionPrefab = explosionPrefab;
    }

    void Start()
    {
        Debug.Log(Player.movement.isGliding);
        if (Player.movement.isGliding)
            Player.movement.ActivateGlider();

        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        hitsRemaining = 3;

        Player.gameObject.GetComponent<CharacterController>().enabled = false;
        Player.stats.elementState = Element.Earth;
        
        Player.stats.ToggleRenderer(false);

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        Player.gameObject.transform.localPosition = Vector3.zero;
        Player.elementGameObject = this.gameObject;

        rb.velocity = Player.movement.currentVelocity;

        Bonk.instance.gameObject.SetActive(false);
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
        
        if (collision.collider.CompareTag("Enemy"))
        {
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            explosion.transform.position = collision.contacts[0].point;
            UnityEngine.Object.Destroy(explosion, 10f);
            // collision.collider.GetComponent<Rigidbody>().AddForce(100f*direction);
            rb.AddForce(-500f*direction*rb.velocity.magnitude);
            collision.collider.gameObject.GetComponent<Enemy>().TakeDamage((int)rb.velocity.magnitude*40/40);

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
        this.gameObject.SetActive(false);
        while (transform.childCount > 0) 
        {
            transform.GetChild(0).SetParent(transform.parent);
        }
        Player.gameObject.transform.rotation = Quaternion.identity;
        Player.stats.elementState = Element.NoElement;
        Player.stats.ToggleRenderer(true);
        Player.elementGameObject = null;

        Player.gameObject.GetComponent<CharacterController>().enabled = true;
        Player.movement.jumpVector = rb.velocity;
        Bonk.instance.gameObject.SetActive(true);
        UnityEngine.Object.Destroy(this.gameObject);
    }
    public Vector3 GetRelativeDirection(Vector3 direction) 
    {
        return Quaternion.LookRotation(new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z)) * direction;
    }
}