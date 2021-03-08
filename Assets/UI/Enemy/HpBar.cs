using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    Vector3 offset;
    Enemy enemy;
    Canvas canvas;
    public Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
        enemy = this.GetComponentInParent<Enemy>();

        offset = this.transform.localPosition;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canvas.enabled)
            canvas.enabled = enemy.health < enemy.maxHealth;

        if (canvas.enabled) {
            hpBar.fillAmount = enemy.health / enemy.maxHealth;
        }
    }
    void LateUpdate()
    {
        this.transform.localPosition = Quaternion.Inverse(transform.parent.rotation) * offset;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0,180,0);
    }
}
