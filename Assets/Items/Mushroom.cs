using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Item
{
    MeshCollider collider;
    Renderer renderer;
    protected override void Start()
    {
        collider = GetComponent<MeshCollider>();
        renderer = GetComponent<Renderer>();
        base.Start();
    }

    public override void OnInteract()
    {
        if (collider.enabled)
        {
            Player.stats.Heal(20);
            StartCoroutine("Respawn");
            collider.enabled = false;
            renderer.enabled = false;
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);
        collider.enabled = true;
        renderer.enabled = true;
    }
}
