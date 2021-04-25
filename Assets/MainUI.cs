using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{   
    public static MainUI instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public static void SetActive(bool value)
    {
        instance.gameObject.SetActive(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
