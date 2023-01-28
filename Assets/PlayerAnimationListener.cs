using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{

    [SerializeField] Animator MechAnimator;
    [SerializeField] Animator SageAnimator;
    [SerializeField] Animator SubAnimator;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerEventPublisher.risingSentEvent  += OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent += OnFallingUpdate;
        PlayerEventPublisher.speedEvent       += OnSpeedUpdate;
        PlayerEventPublisher.ySpeedEvent      += OnYSpeedUpdate;
        PlayerEventPublisher.boostingEvent    += OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent    += OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent   += OnSubmergedUpdate;
        PlayerEventPublisher.onLandEvent      += OnLandUpdate;
        PlayerEventPublisher.jumpEvent        += OnJumpedUpdate;
    }
    
    public void OnRisingUpdate(object source, bool rising){
        SubAnimator.SetBool("Rising",  rising);
    }

    public void OnFallingUpdate(object source, bool falling){
        SubAnimator.SetBool("Falling", falling);
    }

    public void OnTransformToMechUpdate(object source){
       
        SubAnimator.SetTrigger("SubToMech");
        MechAnimator.SetTrigger("Transform");
    }

    public void OnSpeedUpdate(object source, float speed){
        MechAnimator.SetFloat("Speed", speed);
       // SageAnimator.SetFloat("Speed", speed);

    }

    public void OnYSpeedUpdate(object source, float speed){
        MechAnimator.SetFloat("ySpeed", speed);
    }

    public void OnBoostingUpdate(object source){
        MechAnimator.SetTrigger("Boost");
    }
    
    public void OnGroundedUpdate(object source, bool grounded){
        MechAnimator.SetBool("Grounded", grounded);
        // SageAnimator.SetBool("Grounded", grounded);
    }   

    public void OnSubmergedUpdate(object source){
        MechAnimator.SetTrigger("Submerged");
    }

    public void OnLandUpdate(object source){
        SubAnimator.SetTrigger("SubToMech");
        SageAnimator.SetTrigger("Transform");
        MechAnimator.SetTrigger("Transform");
    }

    public void OnJumpedUpdate(object source){
        MechAnimator.SetTrigger("Jump");
        SageAnimator.SetTrigger("Jump");
    }

    public void Test(){}



    void OnDisable()
    {
        PlayerEventPublisher.risingSentEvent  -= OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent -= OnFallingUpdate;
        PlayerEventPublisher.speedEvent       -= OnSpeedUpdate;
        PlayerEventPublisher.ySpeedEvent      -= OnYSpeedUpdate;
        PlayerEventPublisher.boostingEvent    -= OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent    -= OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent   -= OnSubmergedUpdate;
        PlayerEventPublisher.onLandEvent      -= OnLandUpdate;
        PlayerEventPublisher.jumpEvent       -= OnJumpedUpdate;
    }
    
}
