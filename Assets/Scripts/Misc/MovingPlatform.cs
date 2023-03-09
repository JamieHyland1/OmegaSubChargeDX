using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]float speed;
    [SerializeField]Transform startPoint;
    [SerializeField]Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (Vector3.Distance(this.transform.position, startPoint.position) > 0.2f){
            this.transform.position = startPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint.position, speed*Time.deltaTime);
        if(Vector3.Distance(transform.position, endPoint.position) < 0.1f){
            Transform temp = startPoint;
            startPoint = endPoint;
            endPoint = temp;
        }
    }


    void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag != ("Player")) return;
       other.transform.parent = transform;
    }
    void OnTriggerStay(Collider other){
        Debug.Log("Collider " + other);
        if (other.gameObject.tag != ("Player")) return;
        other.transform.parent = transform;
    }

    void OnTriggerExit(Collider other){
        if (other.gameObject.tag != ("Player")) return;
        other.transform.parent = null;
    }
}
