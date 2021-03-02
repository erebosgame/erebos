using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    Player player;
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
        }

        ////
        ///
        

    }
}
