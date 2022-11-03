using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public interface IState
    {
        
        void Enter();
        void Tick();
        void FixedTick();
        void Exit();
        void PrintStateName();
        void EventTrigger();
    }
