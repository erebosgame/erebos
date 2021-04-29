using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthElementItem : ElementItem
{
    public static EarthElementItem instance;

    protected override void Awake()
    {
        instance = this;
    }
    protected override void Start()
    {
        element = Element.Earth;
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
