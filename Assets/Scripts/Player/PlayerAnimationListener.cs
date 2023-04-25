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
        PlayerEventPublisher.risingSentEvent   += OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent  += OnFallingUpdate;
        PlayerEventPublisher.speedEvent        += OnSpeedUpdate;
        PlayerEventPublisher.ySpeedEvent       += OnYSpeedUpdate;
        PlayerEventPublisher.boostingEvent     += OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent     += OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent    += OnSubmergedUpdate;
        PlayerEventPublisher.onLandEvent       += OnLandUpdate;
        PlayerEventPublisher.dashEvent         += OnDashUpdate;
        PlayerEventPublisher.jumpEvent         += OnJumpedUpdate;
        PlayerEventPublisher.swordStatusEvent  += OnSwordStatus;
        PlayerEventPublisher.drawSwordEvent    += OnDrawSword;
        PlayerEventPublisher.sheathSworthEvent += OnSheathSword;
        PlayerEventPublisher.attackEvent       += OnAttackUpdate;
        PlayerEventPublisher.aimEvent          += OnAimUpdate;
        PlayerEventPublisher.directionEvent    += OnDirectionUpdate;

    }
    
    public void OnRisingUpdate(object source, bool rising){
        Debug.Log("Rising  t" + SubAnimator.GetBool("Rising"));
        SubAnimator.SetBool("Rising",  rising);
    }

    public void OnFallingUpdate(object source, bool falling){
        Debug.Log("Falling " + falling);
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

    public void OnDirectionUpdate(object source, Vector2 dir)
    {
        MechAnimator.SetFloat("xDir", dir.x);
        MechAnimator.SetFloat("zDir", dir.y);
    }

    public void OnDashUpdate(object source)
    {
        MechAnimator.SetTrigger("Dash");
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


    public void OnAimUpdate(object source, bool aiming)
    {
        MechAnimator.SetBool("Aim", aiming);
    }

    public void OnLandUpdate(object source){
        SubAnimator.SetTrigger("SubToMech");
        SageAnimator.SetTrigger("Transform");
        MechAnimator.SetTrigger("Transform");
    }

    public void OnAttackUpdate(object source, int attack)
    {
            MechAnimator.SetInteger("Attack", attack);
            // if(attack>-1)
            // {   
                MechAnimator.SetTrigger("Attack_init");
           // }
           Debug.Log("Attack! " + attack);
    }

    public void OnSwordStatus(object source, bool swordEquipped)
    {
        MechAnimator.SetBool("SwordEquipped",swordEquipped);
    }

    public void OnDrawSword(object source)
    {
        MechAnimator.SetTrigger("drawSword");
    }

    public void OnSheathSword(object source)
    {
        MechAnimator.SetTrigger("sheathSword");
    }

    public void OnJumpedUpdate(object source, int jump){
        MechAnimator.SetTrigger("Jump");
        MechAnimator.SetInteger("JumpNumber", jump);
        SageAnimator.SetTrigger("Jump");
    }

    public void Test(){}



    void OnDisable()
    {
        PlayerEventPublisher.risingSentEvent   -= OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent  -= OnFallingUpdate;
        PlayerEventPublisher.speedEvent        -= OnSpeedUpdate;
        PlayerEventPublisher.ySpeedEvent       -= OnYSpeedUpdate;
        PlayerEventPublisher.boostingEvent     -= OnBoostingUpdate;
        PlayerEventPublisher.groundedEvent     -= OnGroundedUpdate;
        PlayerEventPublisher.submergedEvent    -= OnSubmergedUpdate;
        PlayerEventPublisher.onLandEvent       -= OnLandUpdate;
        PlayerEventPublisher.dashEvent         -= OnDashUpdate;
        PlayerEventPublisher.jumpEvent         -= OnJumpedUpdate;
        PlayerEventPublisher.swordStatusEvent  -= OnSwordStatus;
        PlayerEventPublisher.drawSwordEvent    -= OnDrawSword;
        PlayerEventPublisher.sheathSworthEvent -= OnSheathSword;
        PlayerEventPublisher.attackEvent       -= OnAttackUpdate;
        PlayerEventPublisher.aimEvent          -= OnAimUpdate;
    }
    
}
