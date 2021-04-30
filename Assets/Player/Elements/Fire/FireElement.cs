using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

class FireElement : MonoBehaviour
{
    public GameObject explosionPrefab;
    private CharacterController controller;
    Cinemachine.CinemachineCollider cameraCollider;
    
    private Vector3 initialPosition;
    private Vector3 initialDirection;
    private float maxDistance = 30f;
    
    private bool ended;

    void Start()
    {
        GameObject o = Camera.main.GetComponent<Cinemachine.CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        cameraCollider = o.GetComponentInChildren<Cinemachine.CinemachineCollider>();

        print(o);
        print(o.name);
        if (Player.movement.isGliding)
            Player.movement.ActivateGlider();

        if (cameraCollider)
            cameraCollider.enabled = false;
        
        controller = GetComponent<CharacterController>();
        Player.stats.elementState = Element.Fire;
        
        Player.stats.ToggleRenderer(false);

        this.gameObject.transform.SetParent(Player.gameObject.transform.parent);
        Debug.Log("Padre palla: "+ Player.gameObject.transform.parent);
        Player.gameObject.transform.SetParent(this.gameObject.transform);
        //Debug.Log(Player.gameObject.transform.parent);
        Player.elementGameObject = this.gameObject;

        Player.movement.FaceRelativeDirection(new Vector3(0,0,1));
        initialDirection = Camera.main.transform.forward;
        initialPosition = Player.gameObject.transform.position;

        Bonk.instance.gameObject.SetActive(false);
        StartCoroutine("EndAfterTime");
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
        if (other.CompareTag("Enemy") && !other.isTrigger)
        {
            Vector3 direction = (other.transform.position - Player.gameObject.transform.position).normalized;
            if (System.Math.Abs(Vector3.Angle(initialDirection, direction)) > 120)
                return;

            Enemy enemy = other.GetComponent<Enemy>();
            GameObject explosion = UnityEngine.Object.Instantiate(explosionPrefab);
            // GameObject enemyFire = UnityEngine.Object.Instantiate(fireballPrefab, other.transform);
            explosion.transform.position = Player.gameObject.transform.position;
            UnityEngine.Object.Destroy(explosion, 10f);
            // other.GetComponent<Rigidbody>().AddForce(200f*direction);
            enemy.TakeDamage(80);
            End();
        }
    }
    IEnumerator EndAfterTime()
    {
        yield return new WaitForSeconds(5f);
        End();
    }

    void End() 
    {
        if (ended)
            return;
        ended = true;

        if (cameraCollider)
            cameraCollider.enabled = true;

        this.GetComponentInChildren<ParticleSystem>().Stop(true);
        Player.stats.ToggleRenderer(true);
        Player.stats.elementState = Element.NoElement;
        while (transform.childCount > 0) 
        {
            transform.GetChild(0).parent = transform.parent;
        }
        Player.elementGameObject = null;
        Bonk.instance.gameObject.SetActive(true);

        UnityEngine.Object.Destroy(this.gameObject);
    }
}