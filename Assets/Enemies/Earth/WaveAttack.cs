using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 20f;
        this.gameObject.transform.localScale += new Vector3(Time.deltaTime*speed,Time.deltaTime*speed,Time.deltaTime*speed);

        if (this.gameObject.transform.localScale.magnitude >= 1000)
        {
            this.gameObject.SetActive(false);
        }     
    }
    private void OnParticleCollision(GameObject other) {
        if (other.CompareTag("Player"))
        {
            Player.stats.TakeDamage(15);
        }
    }
}
