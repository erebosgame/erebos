using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FireState : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Vector3 direction = (other.transform.position - this.transform.position).normalized;

        if (System.Math.Abs(Vector3.Angle(this.transform.forward, direction)) > 90)
            return;
     
        if (other.tag == "Enemy")
        {
            if(this.transform.parent.GetComponent<Player>().elementState == 1)
                this.transform.parent.GetComponent<StateManager>().FireballTrigger(other);
        }
    }
}
