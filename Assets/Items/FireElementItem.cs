using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireElementItem : ElementItem
{
    public GameObject disabledFireElement;

    protected override void Start()
    {
        element = Element.Fire;
        // this.gameObject.GetComponent<Renderer>().material.color = element.GetColor();
        base.Start();
    }

    public override void OnInteract()
    {
        disabledFireElement.SetActive(true);
        disabledFireElement.transform.position = this.transform.position;
        disabledFireElement.transform.rotation = this.transform.rotation;
        base.OnInteract();
    }
}
