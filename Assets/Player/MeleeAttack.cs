using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    List<Collider> triggerList = new List<Collider>();
    GameObject selected;
    Enemy selectedEnemy;
    Outline selectedOutline;
 
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") || other.isTrigger)
            return;

        if(!triggerList.Contains(other))
        {
            triggerList.Add(other);
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }

    void GetAimed()
    {
        if (selected)
        {
            selectedOutline.OutlineWidth = 0;
            selected = null;
        }
        if (Player.stats.elementState != Element.NoElement)
        {
            return;
        }
        
        List<(float,Collider)> angles = new List<(float, Collider)>();

        foreach (Collider collider in triggerList) {
            if(collider)
                angles.Add((GetColliderAngle(collider),collider));
        }         

        angles.Sort((x,y) => x.Item1.CompareTo(y.Item1));

        if (angles.Count > 0) 
        {
            if (angles[0].Item1 < 80)
            {
                selected = angles[0].Item2.gameObject;
                selectedOutline = selected.GetComponent<Outline>();
                selectedOutline.OutlineWidth = 3;
                selectedEnemy = selected.GetComponent<Enemy>();
            }
        }        
    }

    void Update() 
    {
        GetAimed();
        if (Input.GetKeyDown(KeyCode.Mouse0) && Player.stats.CanUseSkill(Element.NoElement))
        {
            if (selected)
            {
                selectedEnemy.TakeDamage(15);
                Player.stats.UseSkill(Element.NoElement);
            }
        }    
    }

    float GetColliderAngle(Collider collider)
    {
        // TODO: rimuovere  Camera.main
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 playerPos = Player.gameObject.transform.position;
        Vector3 enemyPos = collider.gameObject.transform.position;

        Vector3 playerAngle = (playerPos-cameraPos).normalized;
        Vector3 enemyAngle = (enemyPos-playerPos).normalized;

        return Vector3.Angle(playerAngle,enemyAngle);
    }

}
