using System;

public class PlayerEventPublisher{
    public delegate void PlayerRisingEventHandler  (object source, bool rising);
    public delegate void PlayerFallingEventHandler (object source, bool falling);
    public delegate void PlayerBoostEventHandler   (object source);



    public static event PlayerRisingEventHandler  risingSentEvent;
    public static event PlayerFallingEventHandler fallingSentEvent;
    public static event PlayerBoostEventHandler   boostingEvent;



    public void updateRisingStatus(bool rising){
        risingSentEvent?.Invoke(this, rising);
    }

    public void updateFallingStatus(bool falling){
        fallingSentEvent?.Invoke(this, falling);
    }

    public void updateBoostingStatus(){
        boostingEvent?.Invoke(this);
    }
}