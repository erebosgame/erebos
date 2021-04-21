using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    Canvas canvas;
    public Image hpBar;
    
    // Start is called before the first frame update
    void Start()
    {
        canvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
