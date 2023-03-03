using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 direction;
    public GameObject target;
    private Transform t;
    public Transform _ledgeCheck;
    private LayerMask _groundLayer;

    void Start()
    {
        // this.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        _groundLayer = LayerMask.GetMask("Level Geometry");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit ledgeHit;
        // Vector3 targetDirection = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - transform.position;
        // Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Mathf.PI*2, 0.0f);;
        // transform.rotation = Quaternion.LookRotation(newDirection);
        Debug.Log(Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down * 17.5f, out ledgeHit,_groundLayer));
        this.transform.position = ledgeHit.point;
    }
}
