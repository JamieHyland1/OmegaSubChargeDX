using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRing : MonoBehaviour
{
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        GeneralEventHandler.DashRingEntered(this.transform.forward);
    }
}
