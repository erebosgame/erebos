using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Camera mainCamera;
    List<Collider> triggerList = new List<Collider>();
    GameObject hitEnemyObject;
    Enemy hitEnemy;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Player.stats.CanUseSkill(Element.NoElement))
        {
            for (int i = 0; i < 4; i++)
            {
                Collider collider = GetColliderAtDistance(i);
                if (collider)
                {
                    hitEnemyObject = collider.gameObject;
                    hitEnemy = hitEnemyObject.GetComponent<Enemy>();
                    hitEnemy.TakeDamage(15);
                    Player.stats.UseSkill(Element.NoElement);
                    break;
                }
            }
        }    
    }

    Collider GetColliderAtDistance(float distance)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position - this.transform.forward, distance);        
        List<(float,Collider)> angles = new List<(float, Collider)>();

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Enemy") && !collider.isTrigger)
                angles.Add((GetColliderAngle(collider),collider));
        }         

        angles.Sort((x,y) => x.Item1.CompareTo(y.Item1));

        if (angles.Count > 0) 
        {
            if (angles[0].Item1 < 40)
            {
                return angles[0].Item2;
            }
        }  
        return null;
    }

    float GetColliderAngle(Collider collider)
    {
        Vector3 cameraAngle = mainCamera.transform.forward;
        cameraAngle.y = 0;
        cameraAngle = cameraAngle.normalized;
        Vector3 playerPos = Player.gameObject.transform.position;
        playerPos.y = 0;
        Vector3 enemyPos = collider.gameObject.transform.position;
        enemyPos.y = 0;

        Vector3 enemyAngle = (enemyPos-playerPos).normalized;

        return Vector3.Angle(cameraAngle,enemyAngle);
    }
}
