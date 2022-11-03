
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MoveState : IState
{
        PlayerSM playerSM;
        CharacterController controller;
        public string STATE_NAME = "Move State";
        Transform playerTransform;
        Transform cam;
        Vector2 move;
        Vector3 velocity;
        Vector3 acceleration;
        float acc = 5.5f;
        private PlayerControls controls;
        float walkSpeed;
        float runSpeed;
        float currentSpeed = 5;
        Vector3 direction;
        Vector3 moveDirection;
        float timeTillSpeedUp = 2.5f;
        float speedUpTimer = 0;
        float speed = 0;
        float drag = -0.95f;
        AnimationCurve accelCurve;

        float turnSmoothVelocity;

        float turnSmoothTime;
        float accelTime = 0;
        float decelTime = 1;

        PhysicsHelper helper;
        bool jumpButtonHeld = false;

        LayerMask layer;
        bool sliding = false;
        public MoveState(PlayerSM _playerSM, Transform playerTransform, Transform cam, AnimationCurve accelCurve, PlayerControls controls, ref CharacterController controller, ref Vector2 move, float walkSpeed, float runSpeed, float turnSmoothVelocity, float turnSmoothTime, LayerMask layer, PhysicsHelper helper ){
            playerSM = _playerSM;
           
           
            this.playerTransform = playerTransform;
            this.cam = cam;
            this.accelCurve = accelCurve;
            // this.controls = controls;
            this.controller = controller;
            this.move = move;
            this.walkSpeed = walkSpeed;
            this.runSpeed = runSpeed;
            this.turnSmoothVelocity = turnSmoothVelocity;
            this.turnSmoothTime = turnSmoothTime;
            this.layer = layer;
            this.helper = helper;
        }
        public void Enter()
        {
            controls = new PlayerControls();
            controls.Ground_Move.Enable(); 
            velocity = new Vector2();
            move = new Vector2();
            controls.Ground_Move.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Ground_Move.Move.canceled  += ctx => move = Vector2.zero;
            controls.Ground_Move.Jump.started += ctx => Jump();
            controls.Ground_Move.Jump.canceled += ctx => jumpButtonHeld = false;
            controls.Ground_Move.Dash.performed += ctx => InitiateDash();
            controls.Ground_Move.Slide.started += ctx => InitiateSlide();
            velocity = playerSM.getVelocity();

      //       EventManager.current.OnPlayerTriggerDashUpdate(false);
            if(playerSM.getCurrentSpeed() > walkSpeed) speed = Mathf.Clamp(playerSM.getCurrentSpeed(),0,PhysicsHelper.Instance.afterDashSpeed); else speed = walkSpeed;
           
            //Sometime when changing states, controller input can lag and not register immediately causing player to immediately decellerate
            //capture the direction of the player when leaving a certain state, then compare the magnitude to see which is greater then apply that instead
           
            move.x = playerSM.getDirection().x;
            move.y = playerSM.getDirection().z;
            // controller.height = 8.9f;
            // controller.center = new Vector3(0,4.3f,0);
            // controller.radius = 1.5f;

         

        }


        public void FixedTick()
        {
            
        }

        public void Tick(){          
            if(!playerSM.checkForSlopes()){
                controls.Ground_Move.Disable();
                velocity = playerSM.slideDownSlope(velocity);
            }else{ 
                controls.Ground_Move.Enable();
                

                EventManager.current.OnPlayerTriggerGroundedUpdate(playerSM.isGrounded);

                if(!playerSM.isGrounded)velocity = PhysicsHelper.Instance.applyGravity(velocity,jumpButtonHeld); 
                else if(playerSM.isGrounded && velocity.y < 0) velocity.y = 0;



                Vector3 currDirection = new Vector3(move.x, 0f, move.y);
                direction = currDirection;
                float xDir = playerSM.scale(0, runSpeed, 0, 2, move.x);
                float zDir = playerSM.scale(0, runSpeed, 0, 2, move.y);

             
                EventManager.current.OnPlayerTriggerXDirectionUpdate(move.x * xDir);
                EventManager.current.OnPlayerTriggerYDirectionUpdate(move.y * xDir);

            if (direction.magnitude > 0) { 
             
                float angle = 0;
                PhysicsHelper.Instance.rotateCharacter(direction,playerTransform,cam,turnSmoothTime,turnSmoothVelocity, out moveDirection, out angle);
                
                if(currentSpeed >= speed) speedUpTimer += Time.deltaTime;
                if(speedUpTimer >= timeTillSpeedUp){
                    float x = Mathf.Lerp(PhysicsHelper.Instance.moveSpeed,PhysicsHelper.Instance.runSpeed,accelCurve.Evaluate(accelTime));
                    currentSpeed += x-speed;
                    speed = x;
                    if(!playerSM.system.isPlaying)playerSM.system.Play();
                }  
                if(currentSpeed < speed)currentSpeed += acc * Time.deltaTime; else currentSpeed = speed;
            }
            else{
                if(Vector3.Distance(velocity,Vector3.zero) > 0.1f){
                    //decellerate 4 times at once otherwise its too slow
                    currentSpeed += (currentSpeed * drag) * Time.deltaTime * 3;
                }
                else{
                    accelTime = 0;
                    velocity = Vector3.zero;
                    acceleration = new Vector3();
                    currentSpeed = 0;
                    playerSM.setDirection(Vector3.zero);
                    if(playerSM.system.isPlaying)playerSM.system.Stop();
                    speedUpTimer = 0;
                }
            }
           
            
            // Debug.Log("currentSpeed/Speed %:  "  + Mathf.Lerp(0.5f,1.35f,currentSpeed/speed));
            velocity.x = moveDirection.x * Mathf.Min(currentSpeed,speed);
            velocity.z = moveDirection.z * Mathf.Min(currentSpeed,speed);;

            
         
            accelTime += Time.deltaTime/2;

            }
            controller.Move(velocity * Time.deltaTime);
            float t = 0;
            if(t <= 1.35)t = Mathf.Lerp(0.5f,1.35f,currentSpeed/speed);
            EventManager.current.OnUpdateRunSpeedTrigger(t);
            EventManager.current.OnTriggerPlayerSpeedUpdate(Mathf.Min(currentSpeed,speed));
            
            playerSM.isGrounded = helper.checkGroundCollision(playerSM.isGrounded,playerSM.groundCheck,layer);
            if(!playerSM.isGrounded)playerSM.ChangeState(playerSM.airMoveState);
        }

        public void InitiateDash(){
            playerSM.ChangeState(playerSM.dashState);
        }
        
        public void Exit(){
            playerSM.setDirection(direction);
            playerSM.setVelocity(velocity);
            playerSM.setCurrentSpeed(new Vector3(velocity.x,0f,velocity.y).magnitude);
            controls.Ground_Move.Disable();
        }
  

        public void Jump(){
           
             if(playerSM.isGrounded){
                velocity.y = Mathf.Sqrt(PhysicsHelper.Instance.jumpVelocity * -2f * helper.gravity * 2.5f);
                playerSM.setVelocity(velocity);
                playerSM.ChangeState(playerSM.airMoveState);
                EventManager.current.OnPlayerRegularJumpTrigger();
               //EventManager.current.OnPlayerDashJumpTrigger();
            }
             jumpButtonHeld = true;
            
        }
        public void InitiateSlide(){
            if(playerSM.isGrounded){
                playerSM.setCurrentSpeed(velocity.magnitude);
                playerSM.setVelocity(velocity);
                playerSM.setDirection(direction);
                playerSM.ChangeState(playerSM.slideState);
            }
        }

        public void PrintStateName(){
            Debug.Log(STATE_NAME);
        }

        public void EventTrigger(){}
    }

