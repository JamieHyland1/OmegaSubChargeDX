using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    public void confirmHit(){
        if(waiting) return;
        Time.timeScale = 0;
        StartCoroutine("Stop");
    }

    IEnumerator Stop()
    {
       
        waiting = true;
        yield return new WaitForSecondsRealtime(0.05f);
        //  Debug.Log("HIT CONFIRMED");
        Time.timeScale = 1;
        waiting = false;
    }
}
