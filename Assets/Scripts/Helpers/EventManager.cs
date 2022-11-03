using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class EventManager : MonoBehaviour{
   
    public static EventManager current;
    private void Awake(){
        if(current == null)
            current = this;
        else
            Destroy(obj: this);
    }
    public event Action<Vector3, Vector3> OnPlayerEnter; 
    public event Action<float> OnPlayerSpeedUpdated;
    public event Action<float> OnPlayerXDirectionUpdated;
    public event Action<float> OnPlayerYDirectionUpdated;
    public event Action<Boolean> OnPlayerGroundedUpdated;
    public event Action<Boolean> OnPlayerDashingUpdated;
    public event Action<Boolean> OnPlayerSlidingUpdated;
    public event Action OnPlayerRegularJumpTriggered;
    public event Action OnPlayerDashJumpTriggered;
    public event Action<float> OnPlayerUpdateRunSpeed;
    //this one works as intended
    public void OnPlayerTriggerEnter(Vector3 pos, Vector3 dir){
        if(OnPlayerEnter != null){
            OnPlayerEnter(pos,dir);
        }
    }
    public void OnTriggerPlayerSpeedUpdate(float speed){
        if(OnPlayerSpeedUpdated != null){
            OnPlayerSpeedUpdated?.Invoke(speed);
        }
    }
    public void OnPlayerTriggerXDirectionUpdate(float xDir){
        if(OnPlayerXDirectionUpdated != null){
            OnPlayerXDirectionUpdated(xDir);
        }
    }
    public void OnPlayerTriggerYDirectionUpdate(float xDir){
        if(OnPlayerYDirectionUpdated != null){
            OnPlayerYDirectionUpdated(xDir);
        }
    }
    public void OnPlayerTriggerGroundedUpdate(Boolean isGrounded = false){
        if(OnPlayerGroundedUpdated != null){
            OnPlayerGroundedUpdated(isGrounded);
        }
    }
    public void OnPlayerTriggerDashUpdate(Boolean isDashing = false){
        if(OnPlayerDashingUpdated != null){
            OnPlayerDashingUpdated?.Invoke(isDashing);
        }
    }
    public void OnPlayerTriggerSlidingUpdate(Boolean isSliding = false){
        if(OnPlayerSlidingUpdated != null){
            OnPlayerSlidingUpdated?.Invoke(isSliding);
        }
    }
    public void OnPlayerRegularJumpTrigger(){
        if(OnPlayerRegularJumpTriggered != null){
            OnPlayerRegularJumpTriggered();
        }
    }
    public void OnPlayerDashJumpTrigger(){
        if(OnPlayerDashJumpTriggered != null){
            OnPlayerDashJumpTriggered();
        }
    }
    public void OnUpdateRunSpeedTrigger(float speedValue){
        if(OnPlayerUpdateRunSpeed != null){
            OnPlayerUpdateRunSpeed?.Invoke(speedValue);
        }
    }

    
}
