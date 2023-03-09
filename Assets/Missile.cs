using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 15;
    [SerializeField] private float timeToDie = 1;
    private bool move = false;
    private float time = 0;
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.down * 15f,ForceMode.Impulse);
       
    }

    private void FixedUpdate()
    {
        if(move)
        {
            _rigidbody.AddForce(-this.transform.forward * speed, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (timeToDie <= 0)
        {
            DestroyImmediate(this.gameObject);
        }
        if (_rigidbody.velocity.magnitude <= 0.1f && time >= 0.2f)
        {
            move = true;
        }

        timeToDie -= Time.deltaTime;
        
    }
}
