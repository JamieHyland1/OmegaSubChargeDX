using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour
{
    
    public AudioSource audio;
    public AudioClip whooshSound;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "wave"){
            audio.Play();
            Debug.DrawLine(this.transform.position, this.transform.position + other.transform.forward * 15, Color.white,  0.5f);
        }
        
    }
}
