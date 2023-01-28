using System;

public class PlayerEventPublisher{
    //Submarine events
    public delegate void PlayerRisingEventHandler         (object source, bool rising);
    public delegate void PlayerFallingEventHandler        (object source, bool falling);
    public delegate void PlayerTransformToSubEventHandler (object source);


    //Mech events
    public delegate void PlayerSpeedEventHandler          (object source, float speed);
    public delegate void PlayerYSpeedEventHandler         (object source, float speed);
    public delegate void PlayerBoostEventHandler          (object source);
    public delegate void PlayerGroundedEventHandler       (object source, bool grounded);
    public delegate void PlayerSubmergedEventHandler      (object source);
    public delegate void PlayerOnLandEventHandler         (object source);
    public delegate void PlayerJumpEventHandler           (object source);


    //Submarine events
    public static event PlayerRisingEventHandler         risingSentEvent;
    public static event PlayerFallingEventHandler        fallingSentEvent;
    public static event PlayerTransformToSubEventHandler transformEvent;

    //mech events
    public static event PlayerSpeedEventHandler          speedEvent;
    public static event PlayerYSpeedEventHandler         ySpeedEvent;
    public static event PlayerBoostEventHandler          boostingEvent;
    public static event PlayerGroundedEventHandler       groundedEvent;
    public static event PlayerSubmergedEventHandler      submergedEvent;
    public static event PlayerOnLandEventHandler         onLandEvent;
    public static event PlayerJumpEventHandler           jumpEvent;



    public void updateRisingStatus(bool rising){
        risingSentEvent?.Invoke(this, rising);
    }

    public void updateFallingStatus(bool falling){
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
}