using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    float speed;

    [SerializeField] private Vector3 axis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis.x * speed * Time.deltaTime, axis.y * speed * Time.deltaTime, axis.z, Space.Self);
    }
}
