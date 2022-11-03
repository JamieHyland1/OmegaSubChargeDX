using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    float a = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(a > Mathf.PI) a = 0;
        Vector3 v = transform.position;
        v.y += Mathf.Sin(a) * Mathf.PerlinNoise(Time.deltaTime,a) * Time.deltaTime;
        Debug.Log(v);
        transform.position = v;
        a+= 0.001f;
    }
}
