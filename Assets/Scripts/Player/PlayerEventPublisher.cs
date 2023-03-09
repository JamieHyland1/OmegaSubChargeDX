using System;
using System.Numerics;
using UnityEngine;
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

    public delegate void PlayerDashEventHandler           (object source);
    public delegate void PlayerLockOnEventHandler(object source, bool lockOnPressed);


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

    // Camera delegates
    public delegate void PlayerChangeToSubmarine  (object source);
    public delegate void PlayerChangingToMech     (object source);

    // Camera events
    public static event PlayerChangeToSubmarine playerChangeToSubmarine;
    public static event PlayerChangingToMech    playerChangeToSMech;



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

    public void updateOnLandStatus(){
        onLandEvent?.Invoke(this);
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
}