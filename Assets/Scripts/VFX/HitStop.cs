using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    bool waiting = false;

    private SpringTest springTest;
    // Start is called before the first frame update
    void Start()
    {
        springTest = this.GetComponent<SpringTest>();
    }

    
    public void confirmHit(){
        if(waiting) return;
        Time.timeScale = 0;
        StartCoroutine("Stop");
    }

    IEnumerator Stop()
    {
        springTest.OnHit();
        waiting = true;
        springTest.Pause();
        yield return new WaitForSecondsRealtime(0.05f);
        springTest.Play();
        Debug.Log("HIT CONFIRMED");
        Time.timeScale = 1;
        waiting = false;
    }
}
