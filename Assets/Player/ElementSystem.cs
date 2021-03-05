using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ElementSystem : MonoBehaviour
{    
    public GameObject fireballPrefab;
    public GameObject boulderPrefab;
    public GameObject rockplosionPrefab;
    
    // private IElement currentElement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.stats.elementState == Element.NoElement)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Instantiate(fireballPrefab, Player.gameObject.transform.position, Quaternion.identity);
                // currentElement = new FireElement(fireballPrefab, explosionPrefab);
                // currentElement.Start();
                // (currentElement as FireElement).onEndListener += (() => currentElement = null);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Instantiate(boulderPrefab, Player.gameObject.transform.position, Quaternion.identity);
            //     currentElement = new EarthElement(boulderPrefab, rockplosionPrefab);
            //     currentElement.Start();
            //     (currentElement as EarthElement).onEndListener += (() => currentElement = null);
            }
        }
    }
    
}