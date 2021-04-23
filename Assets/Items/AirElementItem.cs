using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirElementItem : ElementItem
{
    public static AirElementItem instance;

    protected override void Awake()
    {
        instance = this;
        this.enabled = false;
    }
    protected override void Start()
    {
        element = Element.Air;
        // this.gameObject.GetComponent<Renderer>().material.color = element.GetColor();
        base.Start();
    }

    public override void OnInteract()
    {
        base.OnInteract();
    }

    public static void LoadElement()
    {
        instance.OnInteract();
    } 
}
