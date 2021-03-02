using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    Player player;
    GameObject fireball;
    public GameObject fireballPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.elementState = 1;
            player.facing = new Vector3(player.camera.transform.forward.x, 0, player.camera.transform.forward.z);
            player.velocity += player.facing * 40f;
            fireball = Instantiate(fireballPrefab, player.transform);
            player.GetComponent<MeshRenderer>().enabled = false;
        }

        ////
        ///
        

    }

    public void FireballTrigger(Collider collider)
    {
        ParticleSystem parts = fireball.GetComponent<ParticleSystem>();
        parts.Stop();
        player.GetComponent<MeshRenderer>().enabled = true;
        player.elementState = 0;
        player.velocity -= player.facing * 40f;
        Destroy(fireball, parts.main.duration);
        Vector3 direction = (collider.transform.position - this.transform.position).normalized;
        collider.GetComponent<Rigidbody>().AddForce(200f*direction);
    }
}
