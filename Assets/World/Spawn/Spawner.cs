using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public int count;
    public float radius;
    
    public float respawnTime;
    public float respawnPM;

    private int alive;

    void StartRespawnTimer()
    {
        StartCoroutine("RespawnAfterTime");
    }

    IEnumerator RespawnAfterTime()
    {
        yield return new WaitForSeconds(respawnTime + Random.Range(-respawnPM, +respawnPM));
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
        Debug.Log("onspawneddeath");
        alive--;
        if (alive <= 0)
        {
            StartRespawnTimer();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
