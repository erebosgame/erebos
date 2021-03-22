using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject g1;
    public GameObject g2;
    // Start is called before the first frame update
    void Start()
    {
        Matrix4x4 m = Matrix4x4.TRS(g1.transform.localPosition, g1.transform.localRotation, g1.transform.localScale).inverse * Matrix4x4.Rotate(Quaternion.Euler(-90f,0f,0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
