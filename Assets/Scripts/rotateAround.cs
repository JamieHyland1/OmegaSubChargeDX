using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateAround : MonoBehaviour
{
    [SerializeField]
    GameObject body;
     [SerializeField]
    float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.RotateAround(body.transform.position, Vector3.up, speed * Time.deltaTime);
    }
}
