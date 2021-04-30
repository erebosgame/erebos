using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnerGizmos : MonoBehaviour
{

    public Spawner spawner;
    public Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(this.transform.position, 4);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawner.radius);
    }
}
