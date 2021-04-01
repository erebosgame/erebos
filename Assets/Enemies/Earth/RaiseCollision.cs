using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaiseCollision : MonoBehaviour
{
    public EarthEnemyBoss parent;

    void OnTriggerEnter(Collider other)
    {
        parent.OnChildTriggerEnter(GetComponent<MeshCollider>(), other);
    }
    void OnTriggerExit(Collider other)
    {
        parent.OnChildTriggerExit(GetComponent<MeshCollider>(), other);
    }
}

