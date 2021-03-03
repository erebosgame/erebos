using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class FireElement : IElement
{
    private GameObject fireballPrefab;
    private GameObject explosionPrefab;

    private MeshRenderer meshRenderer;

    private GameObject fireball;

    private SphereCollider fireballCollider;

    public event Action onEndListener;
    
    private Vector3 initialPosition;
    private Vector3 initialDirection;

    private float maxDistance = 30f;

    public FireElement(GameObject fireballPrefab, GameObject explosionPrefab)
    {
        this.fireballPrefab = fireballPrefab;
        this.explosionPrefab = explosionPrefab;
        this.meshRenderer = Player.gameObject.GetComponent<MeshRenderer>();
    }


    public void Start()
    {
        Player.stats.elementState = Element.Fire;
        Player.movement.FaceRelativeDirection(new Vector3(0,0,1));
        initialDirection = Player.movement.facingDirection;
        Player.movement.velocity += initialDirection * 40f;
        fireball = UnityEngine.Object.Instantiate(fireballPrefab, Player.gameObject.transform);
        meshRenderer.enabled = false;
        fireballCollider = Player.gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        fireballCollider.radius = 0.75f;
        fireballCollider.isTrigger = true;

        initialPosition = Player.gameObject.transform.position;
    }

    public void Update()
    {
        Vector3 currentPosition = Player.gameObject.transform.position;
        
        if (Vector3.Distance(initialPosition, currentPosition) >= maxDistance)
            End();
    }

    public void Trigger(Collider other)
    {
        Vector3 direction = (other.transform.position - Player.gameObject.transform.position).normalized;
        if (System.Math.Abs(Vector3.Angle(Player.gameObject.transform.forward, direction)) > 90)
            return;
            
        if (other.tag == "Enemy")
        {
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            GameObject enemyFire = UnityEngine.Object.Instantiate(fireballPrefab, other.transform);
            explosion.transform.position = Player.gameObject.transform.position;
            UnityEngine.Object.Destroy(explosion, 10f);
            UnityEngine.Object.Destroy(enemyFire, 10f);
            End();
            //knockback
            other.GetComponent<Rigidbody>().AddForce(200f*direction);
        }
    }

    public void End() 
    {
        fireball.GetComponent<ParticleSystem>().Stop();
        meshRenderer.enabled = true;
        Player.stats.elementState = Element.NoElement;
        Player.movement.velocity -= initialDirection* 40f;
        UnityEngine.Object.Destroy(fireball, 10f);
        UnityEngine.Object.Destroy(fireballCollider);

        if (onEndListener != null) 
        {
            onEndListener();
        }
    }
}