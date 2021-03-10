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
            Collider[] colliders = Physics.OverlapSphere(this.transform.position - this.transform.forward, 4f);        
            List<(float,Collider)> angles = new List<(float, Collider)>();

            foreach (Collider collider in colliders) {
                if (collider.CompareTag("Enemy"))
                    angles.Add((GetColliderAngle(collider),collider));
            }         

            angles.Sort((x,y) => x.Item1.CompareTo(y.Item1));

            if (angles.Count > 0) 
            {
                if (angles[0].Item1 < 40)
                {
                    hitEnemyObject = angles[0].Item2.gameObject;
                    hitEnemy = hitEnemyObject.GetComponent<Enemy>();
                    hitEnemy.TakeDamage(15);
                    Player.stats.UseSkill(Element.NoElement);
                }
            }  
        }    
    }

    float GetColliderAngle(Collider collider)
    {
        Vector3 cameraPos = mainCamera.transform.position;
        cameraPos.y = 0;
        Vector3 playerPos = Player.gameObject.transform.position;
        playerPos.y = 0;
        Vector3 enemyPos = collider.gameObject.transform.position;
        enemyPos.y = 0;

        Vector3 playerAngle = (playerPos-cameraPos).normalized;
        Vector3 enemyAngle = (enemyPos-playerPos).normalized;

        return Vector3.Angle(playerAngle,enemyAngle);
    }
}
