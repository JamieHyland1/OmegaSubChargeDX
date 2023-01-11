using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{

    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerEventPublisher.risingSentEvent  += OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent += OnFallingUpdate;
        PlayerEventPublisher.speedEvent       += OnSpeedUpdate;
        PlayerEventPublisher.boostingEvent    += OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent    += OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent   += OnSubmergedUpdate;
        PlayerEventPublisher.jumpEvent        += OnJumpedUpdate;
    }
    
    public void OnRisingUpdate(object source, bool rising){
        animator.SetBool("Rising",  rising);
    }

    public void OnFallingUpdate(object source, bool falling){
        animator.SetBool("Falling", falling);
    }

    public void OnSpeedUpdate(object source, float speed){
        animator.SetFloat("Speed", speed);

    }

    public void OnBoostingUpdate(object source){
        animator.SetTrigger("Boost");
    }
    
    public void OnGroundedUpdate(object source){
        animator.SetTrigger("Grounded");
    }   

    public void OnSubmergedUpdate(object source){
        animator.SetTrigger("Submerged");
    }

    public void OnJumpedUpdate(object source){
        animator.SetTrigger("Jump");
    }



    void OnDisable()
    {
        PlayerEventPublisher.risingSentEvent  -= OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent -= OnFallingUpdate;
        PlayerEventPublisher.speedEvent       -= OnSpeedUpdate;
        PlayerEventPublisher.boostingEvent    -= OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent    -= OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent   -= OnSubmergedUpdate;
         PlayerEventPublisher.jumpEvent       -= OnJumpedUpdate;
    }
    
}
