using System;

public class PlayerEventPublisher{
    public delegate void PlayerRisingEventHandler    (object source, bool rising);
    public delegate void PlayerFallingEventHandler   (object source, bool falling);
    public delegate void PlayerSpeedEventHandler     (object source, float speed);
    public delegate void PlayerBoostEventHandler     (object source);
    public delegate void PlayerGroundedEventHandler  (object source);
    public delegate void PlayerSubmergedEventHandler (object source);
    public delegate void PlayerJumpEventHandler      (object source);



    public static event PlayerRisingEventHandler    risingSentEvent;
    public static event PlayerFallingEventHandler   fallingSentEvent;
    public static event PlayerSpeedEventHandler     speedEvent;
    public static event PlayerBoostEventHandler     boostingEvent;
    public static event PlayerGroundedEventHandler  groundedEvent;
    public static event PlayerSubmergedEventHandler submergedEvent;
    public static event PlayerJumpEventHandler      jumpEvent;



    public void updateRisingStatus(bool rising){
        risingSentEvent?.Invoke(this, rising);
    }

    public void updateFallingStatus(bool falling){
        fallingSentEvent?.Invoke(this, falling);
    }

    public void updateSpeedStatus(float speed){
        speedEvent?.Invoke(this,speed);
    }

    public void updateBoostingStatus(){
        boostingEvent?.Invoke(this);
    }

    public void updateGroundedStatus(){
        groundedEvent?.Invoke(this);
    }

    public void updateSubmergedStatus(){
        submergedEvent?.Invoke(this);
    }

    public void updateJumpedStatus(){
        jumpEvent?.Invoke(this);
    }
}