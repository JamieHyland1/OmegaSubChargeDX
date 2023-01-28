using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Sound : MonoBehaviour
{
     AudioSource audio;
    public AudioClip whooshSound;
    private float counter = 1;
    private bool canPlay = true;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    
    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.CompareTag("Player")){
            if (canPlay){
                canPlay = false;
                audio.PlayOneShot(whooshSound);
                StartCoroutine(CountDown());
            }
        }
    }

    private IEnumerator CountDown(){
        yield return new WaitForSeconds(counter);
        canPlay = true;
    }

}
