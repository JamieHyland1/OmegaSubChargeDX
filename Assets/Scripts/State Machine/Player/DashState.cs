

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    public class DashState : IState
    {
        readonly String STATE_NAME = "Dash Slide State";
        PlayerSM playerSM;
        Vector3 velocity;
        Transform playerTransform;
        Vector3 nextPos;
        Vector3 velocityCurrent;
        Vector3 dashLocation;
        float animationTimePosition;
        CharacterController controller;
        AnimationCurve dashCurve;
        float speed;
        float dashDistance;
        PhysicsHelper helper;

    public DashState(PlayerSM playerSM, ref CharacterController controller,  AnimationCurve dashCurve, Transform playerTransform, float speed, float DashDistance, PhysicsHelper helper){
        this.playerSM = playerSM;
        this.controller = controller;
        this.dashCurve = dashCurve;
        this.playerTransform = playerTransform;
        this.speed = speed;
        this.dashDistance = DashDistance;
        this.helper = helper;
    }
        public void Enter()
        {
            Debug.Log("In Dash State");
            velocityCurrent = velocity;
            velocity = Vector3.zero;
           
            animationTimePosition = 0;
            float startTime = Time.time;
            nextPos = playerTransform.position;
            Debug.Log(dashDistance);
            dashLocation = playerTransform.position + playerTransform.forward * (dashDistance - controller.radius - controller.skinWidth);
            Ray ray = new Ray(playerTransform.position,playerTransform.forward );
            RaycastHit hit;
            Physics.Raycast(ray,  out hit, dashDistance);
            if(hit.collider != null){
                dashLocation = new Vector3(hit.point.x ,hit.point.y,hit.point.z - controller.radius - controller.skinWidth);
            }
            Debug.Log(dashLocation + " " + dashDistance);

        }

        public void Exit()
        {
        //  EventManager.current.OnPlayerTriggerDashUpdate(false);

        }

        public void FixedTick(){
        }

// TODO add more error checking to dashLocation as character can get stuck on walls
        public void Tick()
        {
            EventManager.current.OnPlayerTriggerDashUpdate(true);
            if(Vector3.Distance(playerTransform.position, dashLocation) > 0.1f){ 
                float amount = dashCurve.Evaluate(animationTimePosition);
                nextPos = Vector3.Lerp(playerTransform.position,dashLocation,(amount));
                if(Physics.CheckSphere(nextPos + Vector3.up*controller.height, controller.height,playerSM.groundLayer))playerSM.RevertState();
                Debug.Log((Physics.CheckSphere(nextPos + Vector3.up*4, controller.height/2,playerSM.groundLayer) + " " + (nextPos + Vector3.up*4))); 
                controller.Move((nextPos-playerTransform.position) * speed * Time.deltaTime);
                float t = Time.deltaTime;
                animationTimePosition += t;

            }else{
              playerSM.ChangeState(playerSM.moveState);
            }
        }

        

        public void PrintStateName(){
            Debug.Log(STATE_NAME);
        }
        // TODO: Create event class that will handle various events for various states
        public void EventTrigger(){
            this.Enter();
            Debug.Log("DASH RING ENCOUNTERED WHILE IN DASH STATE");
        }
        void endDash(){
            animationTimePosition = 0;
            Vector3 vel = playerSM.getVelocity();
            Vector3 velDir = vel.normalized;
            playerSM.setVelocity(new Vector3(velDir.x*PhysicsHelper.Instance.afterDashSpeed,vel.y,velDir.z*PhysicsHelper.Instance.afterDashSpeed));
            playerSM.setCurrentSpeed(PhysicsHelper.Instance.afterDashSpeed);
            velocity = Vector3.zero;
            EventManager.current.OnPlayerTriggerDashUpdate(false);
            playerSM.ChangeState(playerSM.moveState);
        }
    }

