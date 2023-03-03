using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    EventPublisher publisher;
    float timer = 5;
    private void Awake()
    {
        publisher = new EventPublisher();
        
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)publisher.publishMessage();
    }


    // protected virtual void OnPublishMessasge(){
    //     if(messageSent != null)messageSent(this, EventArgs.Empty);else Debug.Log("No subscribers to the event");
    // }
}
