using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = this.gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.green;
        outline.OutlineWidth = 0;
    } 

    virtual public void PickUp()
    {
        Destroy(this.gameObject);
    }
}
