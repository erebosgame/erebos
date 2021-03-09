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
        alive = count;
        for (int i = 0; i < count; i++)
        {
            Vector2 angle = Random.insideUnitCircle;
            float distance = Random.Range(0, radius);
            GameObject e = Instantiate(enemy, this.transform);
            enemy.transform.localPosition = new Vector3(angle.x,0,angle.y) * distance;
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
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
