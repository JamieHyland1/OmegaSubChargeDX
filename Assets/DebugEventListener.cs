using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugEventListener : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentState;

    [SerializeField] private TMP_Text _currentForce;
    
    [SerializeField] private TMP_Text _currentVeloctiy;
    
    [SerializeField] private TMP_Text _submergedStatus;

    

    // Start is called before the first frame update
    void Start()
    {
        PlayerEventPublisher.stateChange += updateState;
        PlayerEventPublisher.forceUpdate += updateForce;
        PlayerEventPublisher.velocityUpdate += updateVelocity;
        PlayerEventPublisher.submergedUpdate += updateSubmerged;
        //_currentForce = "Force: " + "'" + new Vector3() + "'";

    }

    void updateState(object source, string currentState)
    {
        _currentState.text = "Current State: " + currentState;
    }
    void updateForce(object source, Vector3 force)
    {
        _currentForce.text = "Force: " +force;
    }
    void updateVelocity(object source, Vector3 velocity)
    {
        _currentVeloctiy.text = "Velocity: " + velocity;
    }
    void updateSubmerged(object source, bool submerged)
    {
        _submergedStatus.text = "Submerged: " + submerged;
    }
}
