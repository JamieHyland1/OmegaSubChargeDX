using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 15;
    [SerializeField] private float timeToDie = 1;
    [SerializeField] private ParticleSystem explosion;
    private GameObject target = null;
    private bool move = false;
    private float time = 0;
    void Start()
    {
        _rigidbody = this.GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.down * 25f,ForceMode.Impulse);
        explosion.Play();
        Debug.Log("torpedo " + explosion.isPlaying);
        explosion = this.GetComponent<ParticleSystem>();
        explosion.Stop();


    }

    private void FixedUpdate()
    {
        
         MoveMissle();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (timeToDie <= 0)
        {
            explosion.Play();
            
        }

        if (!explosion.isPlaying && timeToDie <= 0)
        {
            DestroyImmediate(this.gameObject);
        }
        if (_rigidbody.velocity.magnitude <= 0.1f && time >= 0.2f)
        {
            move = true;
        }

        timeToDie -= Time.deltaTime;
        
    }

    public void SetTarget(GameObject target)
    {
        Debug.Log("Target set " + target.name);
        this.target = target;
    }

    void MoveMissle()
    {
        if (target == null)
        {
            this.transform.parent = null;
            _rigidbody.AddForce(-this.transform.forward * speed, ForceMode.Impulse);
        }
        else if(target != null)
        {
            this.transform.parent = null;
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            Quaternion targetRotation =(Quaternion.LookRotation(targetDirection));
            transform.rotation = (Quaternion.RotateTowards(( transform.rotation), targetRotation, Time.deltaTime * 360));
            _rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
    }
}
