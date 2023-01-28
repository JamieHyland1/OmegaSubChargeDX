using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubmerged : MonoBehaviour
{

    LayerMask waterLayer;
    bool submerged;
    // Start is called before the first frame update
    void Start()
    {
        waterLayer  = LayerMask.GetMask("Water");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        submerged = ((Physics.Raycast(this.transform.position,  this.transform.TransformDirection(Vector3.up),   out hit,  Mathf.Infinity, waterLayer)
                    &&   !Physics.Raycast(this.transform.position,  this.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, waterLayer)) 
                    ||   (Physics.OverlapSphere(this.transform.position, 0.5f, waterLayer).Length > 0));

        Debug.Log("Up " + Physics.Raycast(this.transform.position,  this.transform.TransformDirection(Vector3.up),   out hit,  Mathf.Infinity, waterLayer) + " Down " + Physics.Raycast(this.transform.position,  this.transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, waterLayer) + " Sphere " + (Physics.OverlapSphere(this.transform.position, 0.5f, waterLayer).Length > 0) + " submerged " + submerged);
    }
}
