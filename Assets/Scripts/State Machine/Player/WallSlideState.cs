using System;
using System.Collections.Generic;
using UnityEngine;



    public class WallSlideState : IState
    {
        readonly String STATE_NAME = "Wall Slide State";
        PhysicsHelper helper;
        PlayerSM playerSM;
        Transform playerTransform;
        Transform groundCheck;
        Transform wallCheck;
        PlayerControls controls;
        CharacterController controller;
        LayerMask layer;

        Vector3 velocity;
       
        bool onWall;
        Vector3 normal;
        float timer = 2f;
        float counter = 0;
        public WallSlideState(PlayerSM playerSM, Transform playerTransform, Transform groundCheck, Transform wallCheck, PlayerControls controls, ref CharacterController controller, LayerMask layer, PhysicsHelper helper){
            this.playerSM = playerSM;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.wallCheck = wallCheck;
            // this.controls = controls;
            this.controller = controller;
            this.layer = layer;
           
            this.helper = helper;
        }

        public void Enter(){
          controls = new PlayerControls();
            normal = new Vector3();
            velocity = playerSM.getVelocity();
            controls.Enable();
            controls.WallJump.Jump.performed += ctx => wallJump();
            counter = timer;
            // Debug.Log("Entered wall move state from " );
            // playerSM.previousState.PrintStateName();
        }

       
        public void FixedTick(){
        }

        public void Tick(){
            onWall   = PhysicsHelper.Instance.checkPos(wallCheck.position, 2f, layer);
            
            
            
            playerSM.isGrounded = PhysicsHelper.Instance.checkGroundCollision(playerSM.isGrounded,groundCheck,layer);
            RaycastHit hit = PhysicsHelper.Instance.getHitInfo(wallCheck.position,playerTransform.forward,5f,layer);
            if(hit.collider != null)normal = hit.normal;
            
            
            //TODO:
            //originally wanted a counter here to count down the time you didnt hit a wall
            //once the counter hit 0 you would change to AirMoveState which would allow you to move
            //again in the air but it was causing weird errors, need to test further in the future :) 

            // counter -= Time.deltaTime;
            // if(counter <= 0)playerSM.ChangeState(playerSM.airMoveState);
            
            
            
            velocity = PhysicsHelper.Instance.applyGravity(velocity,true);
            Debug.DrawLine(wallCheck.position,wallCheck.position+(playerTransform.forward*2),Color.black,0.2f);
    
            controller.Move(velocity * Time.deltaTime);
            

            //if you hit the floor during a wall jump reset player velocity so they dont
            //slide around after hitting the ground
            if(playerSM.isGrounded == true){
                velocity = Vector3.zero;
                playerSM.ChangeState(playerSM.moveState);
            }
        }


         public void Exit(){
            controls.WallJump.Disable();
            playerSM.setVelocity(Vector3.zero);
            playerSM.setCurrentSpeed(0);
            playerSM.setDirection(Vector3.zero);
        }

        public void PrintStateName(){
            Debug.Log(STATE_NAME);
        }
        public void wallJump(){
            if(onWall && !playerSM.isGrounded){
               //change to eventmanager method
                // animator.SetTrigger("Jump");
                velocity =  (Vector3.up * 45) + (normal * 55);
                 Debug.Log("Wall jump " + velocity );
                Debug.Log("Normal " + normal );
                playerTransform.rotation = Quaternion.FromToRotation(playerTransform.forward,normal) * playerTransform.rotation;
            }
        }

        public void EventTrigger(){}
    }

