using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEventHandler : MonoBehaviour
{
     public delegate void OnPlayerEnterDashRingHandler (Vector3 direction);
     public static event OnPlayerEnterDashRingHandler onPlayerEnterDashRing;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void DashRingEntered(Vector3 direction){
        onPlayerEnterDashRing?.Invoke(direction);
    }
}
