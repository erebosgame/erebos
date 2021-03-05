using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class FireElement : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    public GameObject explosionPrefab;
    private CharacterController controller;
    
    private Vector3 initialPosition;
    private Vector3 initialDirection;

    private float maxDistance = 30f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Player.stats.elementState = Element.Fire;
        
        meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        
        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);

        Player.movement.FaceRelativeDirection(new Vector3(0,0,1));
        initialDirection = Player.movement.facingDirection;
        initialPosition = Player.gameObject.transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = Player.gameObject.transform.position;
        
        if (Vector3.Distance(initialPosition, currentPosition) >= maxDistance)
            End();

        controller.Move(initialDirection * 40f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
            
        if (other.tag == "Enemy")
        {
            Vector3 direction = (other.transform.position - Player.gameObject.transform.position).normalized;
            print(System.Math.Abs(Vector3.Angle(Player.gameObject.transform.forward, direction)));
            if (System.Math.Abs(Vector3.Angle(Player.gameObject.transform.forward, direction)) > 120)
                return;
            print("test");
            Enemy enemy= other.GetComponent<Enemy>();
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            // GameObject enemyFire = UnityEngine.Object.Instantiate(fireballPrefab, other.transform);
            explosion.transform.position = Player.gameObject.transform.position;
            UnityEngine.Object.Destroy(explosion, 10f);
            other.GetComponent<Rigidbody>().AddForce(200f*direction);
            enemy.TakeDamage(40);
            End();
        }
    }

    void End() 
    {
        this.GetComponentInChildren<ParticleSystem>().Stop(true);
        meshRenderer.enabled = true;
        Player.stats.elementState = Element.NoElement;
        transform.DetachChildren();
        UnityEngine.Object.Destroy(this.gameObject);
    }
}