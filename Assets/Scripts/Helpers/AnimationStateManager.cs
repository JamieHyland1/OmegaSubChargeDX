using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateManager : MonoBehaviour
{
    public Animator animator;
    int speedCounter = 0;
    int dashCounter = 0;

    // Subscribe to events
    public void Start(){
        EventManager.current.OnPlayerGroundedUpdated += SetPlayerGrounded;
        EventManager.current.OnPlayerDashingUpdated += SetPlayerDashing;
        EventManager.current.OnPlayerSlidingUpdated += SetPlayerSliding;
        EventManager.current.OnPlayerSpeedUpdated += SetPlayerSpeed;
        EventManager.current.OnPlayerXDirectionUpdated += SetPlayerXDir;
        EventManager.current.OnPlayerYDirectionUpdated += SetPlayerYDir;
        EventManager.current.OnPlayerRegularJumpTriggered += TriggerRegularJump;
        EventManager.current.OnPlayerDashJumpTriggered += TriggerDashJump;
        EventManager.current.OnPlayerUpdateRunSpeed += UpdateRunSpeed;
        
    }   

    // Unsubscribe to events
    public void OnDestroy(){
        EventManager.current.OnPlayerGroundedUpdated -= SetPlayerGrounded;
        EventManager.current.OnPlayerDashingUpdated -= SetPlayerDashing;
        EventManager.current.OnPlayerSlidingUpdated -= SetPlayerSliding;
        EventManager.current.OnPlayerSpeedUpdated -= SetPlayerSpeed;
        EventManager.current.OnPlayerXDirectionUpdated -= SetPlayerXDir;
        EventManager.current.OnPlayerYDirectionUpdated -= SetPlayerYDir;
        EventManager.current.OnPlayerUpdateRunSpeed -= UpdateRunSpeed;
    }


    private void SetPlayerGrounded(Boolean isGrounded){
        animator.SetBool("IsGrounded", isGrounded);
    }
    private void SetPlayerDashing(Boolean isDashing){
        animator.SetBool("IsDashing",isDashing);
    }
    private void SetPlayerSliding(Boolean isSliding){
       animator.SetBool("IsSliding", isSliding);
      // if(!isSliding)animator.SetTrigger("ExitSlide"); 
    }
    private void SetPlayerSpeed(float speed){
//        Debug.Log(speed + " speed");
        animator.SetFloat("Speed", speed);
    }
    private void SetPlayerXDir(float xDir){
     //   animator.SetFloat("Velocity X", xDir);
    }
    private void SetPlayerYDir(float yDir){
       // animator.SetFloat("Velocity Z", yDir);
    }
    private void SetPlayerJumpTriggerRegular(float yDir){
       // animator.SetFloat("Velocity Z", yDir);
    }
    private void TriggerRegularJump(){
        animator.SetTrigger("Jump_Regular");
    }
    private void TriggerDashJump(){
        animator.SetTrigger("Jump_Dash");
    }
    private void UpdateRunSpeed(float speedValue){
        animator.speed = speedValue;
    }
}
