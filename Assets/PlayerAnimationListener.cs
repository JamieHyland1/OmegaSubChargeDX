using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationListener : MonoBehaviour
{

    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerEventPublisher.risingSentEvent += OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent += OnFallingUpdate;
        PlayerEventPublisher.boostingEvent += OnBoostingUpdate;
    }
    public void OnRisingUpdate(object source, bool rising){
        animator.SetBool("Rising",  rising);
    }
    public void OnFallingUpdate(object source, bool falling){
        animator.SetBool("Falling", falling);
    }

    public void OnBoostingUpdate(object source){
        animator.SetTrigger("Boost");
    }   


    void OnDisable()
    {
        PlayerEventPublisher.risingSentEvent -= OnRisingUpdate;
        PlayerEventPublisher.fallingSentEvent -= OnFallingUpdate;
        PlayerEventPublisher.boostingEvent -= OnBoostingUpdate;
    }
    
}
