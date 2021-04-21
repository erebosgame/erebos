using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, Targetable, Interactable
{
    private Outline outline;

    protected virtual void Start()
    {
        outline = GetComponent<Outline>();
        if (outline)
        {
            outline.OutlineMode = Outline.Mode.OutlineAll;
            // outline.OutlineColor = Color.green;
            outline.OutlineWidth = 0;
        }
    } 
    
    public void OnTargetStart() 
    { 
        if (outline)
            outline.OutlineWidth = 8; 
    }
    public void OnTargetStop() 
    { 
        if (outline)
            outline.OutlineWidth = 0; 
    }
    public KeyCode GetInteractKey()
    {
        return KeyCode.F;
    }

    public virtual void OnInteract()
    {
        Destroy(this.gameObject);
    }
}
