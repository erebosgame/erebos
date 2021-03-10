using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementItem : Item
{
    public Element element;

    protected override void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = element.GetColor();
        base.Start();
    }

    public override void OnInteract()
    {
        Player.stats.UnlockElement(element);
        base.OnInteract();
    }
}
