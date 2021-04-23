using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEgg : MonoBehaviour
{
    public bool onTerrain;
    public AirBoss boss;

    // Start is called before the first frame update
    void Start()
    {
        onTerrain = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && onTerrain)
        {
            boss.MoveEgg();
            onTerrain = false;
        }
    }

    public void EnableEgg()
    {
        GetComponent<ElementItem>().enabled = true;
        GetComponent<Outline>().enabled = true;
    }
}
