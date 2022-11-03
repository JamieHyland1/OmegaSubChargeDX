
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other){
        Debug.Log(this.gameObject.name + " " + "COLLIDED WITH " + other.gameObject.name);
        if(other.gameObject.name == "Player")EventManager.current.OnPlayerTriggerEnter(transform.position,transform.forward);
        
    }
    void Update(){
        Debug.DrawLine(transform.transform.position,(transform.transform.position)+transform.forward*5,Color.blue,5);
    }
    void OnDrawGizmos(){
        if(Application.isPlaying){
            Gizmos.DrawLine(transform.transform.position,transform.position+transform.forward*5);
        }    
    }
}
