using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public float force = 50f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !collider.isTrigger)
        {
            Player.gameObject.GetComponent<PlayerMovement>().jumpVector += new Vector3(0, force, 0f);
        }
    }
}
