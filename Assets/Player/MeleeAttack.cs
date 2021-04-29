using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    Camera mainCamera;
    List<Collider> triggerList = new List<Collider>();
    GameObject hitEnemyObject;
    Damageable hitEnemy;

    public GameObject ammo;
    public GameObject cameraRotator;

    public Bonk bonk;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(Player.stats.weapon && Player.stats.CanUseSkill(Element.NoElement))
            {
                bonk.PerformAttack();
                for (int i = 0; i < 4; i++)
                {
                    Collider collider = GetColliderAtDistance(i);
                    if (collider)
                    {
                        hitEnemyObject = collider.gameObject;
                        hitEnemy = hitEnemyObject.GetComponent<Damageable>();
                        Debug.Log(hitEnemy);
                        hitEnemy.TakeDamage(15);
                        Player.stats.UseSkill(Element.NoElement);
                        break;
                    }
                }
            }
            else if (!Player.stats.weapon && (Time.time - Player.stats.lastSlingUse) > Player.stats.slingCooldown)
            {
                Player.stats.lastSlingUse = Time.time;
                FireSling();
            }
        }    
    }

    Collider GetColliderAtDistance(float distance)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position - this.transform.forward, distance);        
        List<(float,Collider)> angles = new List<(float, Collider)>();

        angles.AddRange(colliders
            .Where(c => c.CompareTag("Enemy") && !c.isTrigger)
            .Select(c => (GetColliderAngle(c), c)));

     /*   foreach (Collider collider in colliders) {  
            if (collider.CompareTag("Enemy") && !collider.isTrigger)
                angles.Add((GetColliderAngle(collider),collider));
        }        */ 

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

    void FireSling()
    {
        GameObject projectile = Instantiate(ammo, Player.gameObject.transform.position, Quaternion.identity);

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 target;
        Physics.Raycast(ray, out hit, 1000f, ~LayerMask.GetMask(), QueryTriggerInteraction.Ignore);
        if (hit.collider)
            target = hit.point;
        else
            target = ray.GetPoint(1000);

        Vector3 direction = (target - projectile.transform.position).normalized;

        projectile.transform.forward = direction;

        projectile.GetComponent<SlingShot>().Fire(direction);
    }
}
