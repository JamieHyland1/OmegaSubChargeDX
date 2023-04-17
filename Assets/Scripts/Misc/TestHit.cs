using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour
{
    
    public AudioSource audio;
    public AudioClip whooshSound;
    private HitStop hitStop;
    private SpringTest springTest;


    void Start()
    {
        hitStop = this.GetComponent<HitStop>();
        springTest = this.GetComponent<SpringTest>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colission " + other.gameObject.name);
        hitStop.confirmHit();

    }



    private void OnParticleCollision(GameObject other)
    {
        // springTest.OnHit();
        hitStop.confirmHit();
        
        Debug.Log("Enemy Hit");
    }
}
