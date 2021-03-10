using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementItem : Item
{
    public Element element;

    void Start()
    {
        this.gameObject.GetComponent<Renderer>().material.color = element.GetColor();
    }

    override public void PickUp()
    {
        Player.stats.UnlockElement(element);
        base.PickUp();
    }
}
