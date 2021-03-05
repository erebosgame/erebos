using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.localPosition;
        this.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.GetComponentInParent<Enemy>().health < 100)
        {
            this.GetComponent<Canvas>().enabled = true;
        }
    }

    void LateUpdate()
    {
        this.transform.localPosition = Quaternion.Inverse(transform.parent.rotation) * offset;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0,180,0);
    }
}
