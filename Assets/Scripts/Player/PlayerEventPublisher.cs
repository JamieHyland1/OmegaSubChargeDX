using System;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerEventPublisher{
    // Submarine delegates
    public delegate void PlayerRisingEventHandler         (object source, bool rising);
    public delegate void PlayerFallingEventHandler        (object source, bool falling);
    public delegate void PlayerTransformToSubEventHandler (object source);


    // Mech delegates
    public delegate void PlayerSpeedEventHandler          (object source, float speed);
    public delegate void PlayerYSpeedEventHandler         (object source, float speed);
    public delegate void PlayerBoostEventHandler          (object source);
    public delegate void PlayerGroundedEventHandler       (object source, bool grounded);
    public delegate void PlayerSubmergedEventHandler      (object source);
    public delegate void PlayerOnLandEventHandler         (object source);
    public delegate void PlayerJumpEventHandler           (object source);
    
    public delegate void PlayerLockOnEventHandler(object source, bool lockOnPressed);

    public delegate void PlayerDashEventHandler(object source);

    public delegate void PlayerSwordStatusEventHandler(object source, bool swordEquipped);

    public delegate void PlayerDrawSwordEventHandler(object source);

    public delegate void PlayerSheathSwordEventHandler(object source);

    public delegate void PlayerAttackEventHandler(object source, int attack);

    public delegate void PlayerAttackInitEventHandler(object source);

    public delegate void PlayerAimEventHandler(object source, bool aiming);

    public delegate void PlayerDirectionEventHandler(object source, Vector2 direction);



    // Submarine events
    public static event PlayerRisingEventHandler         risingSentEvent;
    public static event PlayerFallingEventHandler        fallingSentEvent;
    public static event PlayerTransformToSubEventHandler transformEvent;

    // Mech events
    public static event PlayerSpeedEventHandler          speedEvent;
    public static event PlayerYSpeedEventHandler         ySpeedEvent;
    public static event PlayerBoostEventHandler          boostingEvent;
    public static event PlayerGroundedEventHandler       groundedEvent;
    public static event PlayerSubmergedEventHandler      submergedEvent;
    public static event PlayerOnLandEventHandler         onLandEvent;
    public static event PlayerJumpEventHandler           jumpEvent;
    public static event PlayerDashEventHandler           dashEvent;
    public static event PlayerLockOnEventHandler         lockEvent;
    
    public static event PlayerSwordStatusEventHandler      swordStatusEvent;

    public static event PlayerDrawSwordEventHandler      drawSwordEvent;

    public static event PlayerSheathSwordEventHandler   sheathSworthEvent;
    public static event PlayerAttackEventHandler        attackEvent;

    public static event PlayerAimEventHandler           aimEvent;

    public static event PlayerDirectionEventHandler     directionEvent;
    

    // Camera delegates
    public delegate void PlayerChangeToSubmarine  (object source);
    public delegate void PlayerChangingToMech     (object source);

    // Camera events
    public static event PlayerChangeToSubmarine playerChangeToSubmarine;
    public static event PlayerChangingToMech    playerChangeToSMech;
    
    
    //Debug delegates
    public delegate void PlayerStateChange(object source, string state);

    public delegate void PlayerForceUpdate(object source, Vector3 force);

    public delegate void PlayerVelocityUpdate(object source, Vector3 velocity);

    public delegate void PlayerSubmergedUpdate(object souce, bool submerged);
    
    // Debug events
    public static event PlayerStateChange stateChange;
    public static event PlayerForceUpdate forceUpdate;
    public static event PlayerVelocityUpdate velocityUpdate;
    public static event PlayerSubmergedUpdate submergedUpdate;
    
    



    public void updateRisingStatus(bool rising){
        // Debug.Log("Rising eventPub " + falling);
        risingSentEvent?.Invoke(this, rising);
    }

    public void updateFallingStatus(bool falling){
        // Debug.Log("Falling eventPub " + falling);
        fallingSentEvent?.Invoke(this, falling);
    }

    public void updateTransformStatus(){
        transformEvent?.Invoke(this);
    }

    public void updateSpeedStatus(float speed){
        speedEvent?.Invoke(this,speed);
    }

    public void updateYSpeedStatus(float speed){
        ySpeedEvent?.Invoke(this,speed);
    }

    public void updateBoostingStatus(){
        boostingEvent?.Invoke(this);
    }

    public void updateGroundedStatus(bool grounded){
        groundedEvent?.Invoke(this, grounded);
    }

    public void updateSubmergedStatus(){
        submergedEvent?.Invoke(this);
    }

    public void updateAttackStatus(int attack)
    {
        attackEvent?.Invoke(this,attack);
    }

    public void UpdateDirection(Vector2 dir)
    {
        directionEvent?.Invoke(this,dir);
    }
    public void updateOnLandStatus(){
        onLandEvent?.Invoke(this);
    }

    public void updateSwordStatus(bool swordEquipped)
    {
        swordStatusEvent?.Invoke(this, swordEquipped);
    }
    
    public void updateDrawSword()
    {
        drawSwordEvent?.Invoke(this);
    }

    public void updateAimStatus(bool aiming)
    {
        aimEvent?.Invoke(this,aiming);
    }

    public void updateSheathSword()
    {
        sheathSworthEvent?.Invoke(this);
    }

    public void updateJumpedStatus(){
        jumpEvent?.Invoke(this);
    }

    public void changeToSubCamera(){
        playerChangeToSubmarine?.Invoke(this);
    }

    public void changeToMechCamera(){
        playerChangeToSMech?.Invoke(this);
    }

    public void playDashEvent() {
        dashEvent?.Invoke(this);
    }

    public void targetEnemy(bool lockOn)
    {
        lockEvent?.Invoke(this,lockOn);
    }

    public void updateStateChange(string state)
    {
        stateChange?.Invoke(this,state);
    }

    public void updateForce(Vector3 force)
    {
        forceUpdate?.Invoke(this,force);
    }

    public void updateVelocity(Vector3 velocity)
    {
        velocityUpdate?.Invoke(this,velocity);
    }

    public void updateSubmerged(bool submerged)
    {
        submergedUpdate?.Invoke(this,submerged);
    }
}