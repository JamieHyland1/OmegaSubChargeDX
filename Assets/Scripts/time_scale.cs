using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class time_scale : MonoBehaviour
{
    [Range(0.0f, 1.0f)] 
    public float setTimeScale = 1;
    

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = setTimeScale;
    }
}
