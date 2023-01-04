using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    // Start is called before the first frame update

    bool eventRevieved = false;
    EventPublisher pub = new EventPublisher();
  //  public event EventHandler eventRecieved;


    void Start()
    {
        EventPublisher.messageSent += OnMessagePublishedEventHandler;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Test " + eventRevieved);
    }

    public void OnMessagePublishedEventHandler(object source, bool test){
        
        eventRevieved = test;
    }
}
