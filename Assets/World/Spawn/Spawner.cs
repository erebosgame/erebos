using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    private SphereCollider sphereCollider;
    public int count;
    public float radius;
    public float respawnTime;
    public float respawnPM;

    private int alive;
    private bool canSpawn;

    void StartRespawnTimer()
    {
        StartCoroutine("RespawnAfterTime");
    }

    IEnumerator RespawnAfterTime()
    {
        yield return new WaitForSeconds(respawnTime + Random.Range(-respawnPM, +respawnPM));
        if(canSpawn)
            Spawn();
    }

    void Spawn()
    {
        RaycastHit hit;
        alive = count;
        for (int i = 0; i < count; i++)
        {
            Vector2 angle = Random.insideUnitCircle;
            float distance = Random.Range(0, radius);
            GameObject e = Instantiate(enemy, this.transform);
            Vector3 position = new Vector3(angle.x,0,angle.y) * distance;
            Physics.Raycast(position, Vector3.up, out hit, 1000f, LayerMask.GetMask("Terrain"));
            if (hit.collider != null)
                position = hit.point + Vector3.up * 3;
            enemy.transform.localPosition = position;
        }
    }

    public void OnSpawnedDeath()
    {
        alive--;
        if (alive <= 0)
        {
            StartRespawnTimer();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 1500F;
        sphereCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !canSpawn)
        {
            canSpawn = true;
            Spawn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && canSpawn)
        {
            canSpawn = false;
            Despawn();
        }
    }

    private void Despawn()
    {
        foreach(Transform child in transform)
        {
            //Destroy(child.gameObject);
            alive--;
        }
    }
}
