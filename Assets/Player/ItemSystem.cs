using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSystem : MonoBehaviour
{
    Camera mainCamera;
    List<Collider> triggerList = new List<Collider>();
    GameObject selected;
    Item selectedItem;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Item") || other.isTrigger)
            return;

        if (!triggerList.Contains(other))
        {
            triggerList.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }

    void GetAimed()
    {
        if (selected)
        {
            selectedItem.OnTargetStop();
            selected = null;
            selectedItem = null;
        }
        if (Player.stats.elementState != Element.NoElement)
        {
            return;
        }

        List<(float, Collider)> angles = new List<(float, Collider)>();

        foreach (Collider collider in triggerList)
        {
            if (collider && collider.CompareTag("Item") && !collider.isTrigger)
                angles.Add((GetColliderAngle(collider), collider));
        }

        angles.Sort((x, y) => x.Item1.CompareTo(y.Item1));

        if (angles.Count > 0)
        {
            selected = angles[0].Item2.gameObject;
            selectedItem = selected.GetComponent<Item>();
            selectedItem.OnTargetStart();
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
            Player.stats.weapon = !Player.stats.weapon;
        GetAimed();
        if (selectedItem && Input.GetKeyDown(selectedItem.GetInteractKey()))
        {
            selectedItem.OnInteract();
        }
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
