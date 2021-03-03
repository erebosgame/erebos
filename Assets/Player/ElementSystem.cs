using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ElementSystem : MonoBehaviour
{    
    public GameObject fireballPrefab;
    public GameObject explosionPrefab;
    private IElement currentElement;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentElement == null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentElement = new FireElement(fireballPrefab, explosionPrefab);
                currentElement.Start();
                (currentElement as FireElement).onEndListener += (() => currentElement = null);
            }
        }

        if (currentElement != null)
        {
            currentElement.Update();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (currentElement != null)
        {
            currentElement.Trigger(other);
        }
    }
}