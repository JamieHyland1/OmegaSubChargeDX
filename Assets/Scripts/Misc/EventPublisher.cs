using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPublisher
{
    // Start is called before the first frame update
    public delegate void MessagePublishedEventHandler(object source, bool test);
    public static event MessagePublishedEventHandler messageSent;
    float timer = 5;
  
    protected virtual void OnPublishMessasge(){
        if(messageSent != null)messageSent(this, true);else Debug.Log("No subscribers to the event");
    }


    public void publishMessage(){
        OnPublishMessasge();
    }
}
