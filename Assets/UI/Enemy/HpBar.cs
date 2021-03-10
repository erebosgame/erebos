using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    private Canvas canvas;
    public Image hpBar;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
        offset = this.transform.localPosition;
        canvas.enabled = false;
    }
    void LateUpdate()
    {
        // TODO: rinuovere Camera.main
        this.transform.localPosition = Quaternion.Inverse(transform.parent.rotation) * offset;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0,180,0);
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        if(!canvas.enabled)
            canvas.enabled = health < maxHealth;

        if (canvas.enabled) {
            hpBar.fillAmount = (float)health / maxHealth;
        }
    }
}
