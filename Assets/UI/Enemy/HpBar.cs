using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{    public float size = 0.02f;
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
        transform.localScale = new Vector3(transform.localScale.x*size/transform.lossyScale.x,
                                            transform.localScale.y*size/transform.lossyScale.y,
                                            transform.localScale.z*size/transform.lossyScale.z);
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        UpdateHealth(health, maxHealth, false);
    }
    public void UpdateHealth(int health, int maxHealth, bool forceShow)
    {
        canvas.enabled = health < maxHealth || forceShow;

        if (canvas.enabled) {
            hpBar.fillAmount = (float)health / maxHealth;
        }
    }
}
