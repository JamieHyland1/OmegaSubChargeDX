
using System.Data.SqlTypes;
using UnityEngine.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MoveState : IState
{
    PlayerSM playerSM;
    PlayerControls controls;
    Transform playerTransform;
    Transform groundCheck;
    LayerMask waterLayer;
    LayerMask groundLayer;
    GameObject boostEffectObj;
    AnimationCurve accelCurve;
    Material attackMat;
    Rigidbody rigidbody;
    Camera camera;
    PlayerEventPublisher publisher;
    BoxCollider collider;

    Vector2 move;
    Vector2 mouseMove;
    Vector3 xRotation;
    Vector3 yRotation;
    Vector3 prevPos;
    Vector3 force;
   
    float throttle = 0;
    float rise;
    float fall;
    float dashTimer = 1.25f;
    float counter = 0;
    float speed;
    float moveSpeed;
    float ySpeed;
    float accelTime;
    float fallTriggerDeadzone;
    float dashSpeed;
    float currentSpeed = 0;
    float jumpForce = 100;
    float speedTarget;
    float turnSmoothVelocity = 6000;
    float turnSmoothTime = 0.3f;
    public static float turbo = 1;
    float angle;
    float targetAngle;

    // bool isDashing = false;
    bool aboveWater = false;
    bool isBoosting = false;
    bool inMenu = false;
    bool rising = false;
    bool falling = false;
    bool jump = false;
    
    int boostTime = 1;
    int jumpPresesed = 0;
    

    
      
        public MoveState(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform, Transform groundCheck,  PlayerControls controls,  Material attackMat, GameObject boostEffectObj, float fallTriggerDeadzone, float moveSpeed, float ySpeed, float dashSpeed, AnimationCurve accelCurve, BoxCollider collider){
            playerSM = _playerSM;
            this.rigidbody = rigidbody;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.controls = controls;
           
            this.attackMat = attackMat;
            this.boostEffectObj = boostEffectObj;
            this.fallTriggerDeadzone = fallTriggerDeadzone;
            this.moveSpeed = moveSpeed;
            this.dashSpeed = dashSpeed;
            this.ySpeed = ySpeed;
            this.accelCurve = accelCurve;
            this.collider = collider;
        }
        
        
        public void Enter(){
            collider.center = new Vector3(0f,2.5f,-0.77f);
            collider.size = new Vector3(5f,5f,8.5f);
            move = new Vector2();   
            angle = 0;
            accelTime = 0;
            waterLayer  = LayerMask.GetMask("Water");
            groundLayer = LayerMask.GetMask("Level Geometry");
            camera = Camera.main;
            publisher = new PlayerEventPublisher();
            
        }


        public void FixedTick(){
            prevPos = playerTransform.position;
            // Y rotation based on camera
            float targetAngle = Mathf.Atan2(move.x, 0) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            rigidbody.MoveRotation(Quaternion.Euler(0,angle  * Time.timeScale,0));
            if(aboveWater)force.y -= 9.8f;
            else force = ((Vector3.up * ySpeed * (rise + -fall)) +  (playerTransform.forward * speed * throttle));
            if(jump == true && currentSpeed > 200){
                Debug.Log("Jump " + jump);
                rigidbody.AddForce(Vector3.up * 5,ForceMode.Impulse);   
                jump = false; 
            }
            rigidbody.AddForce(force);
        }

        public void Tick(){          
            //check if player is above the water line or not
            //TODO check if player is above water using a raycast pointing downward

             RaycastHit hit;
             RaycastHit groundHit;

            Debug.Log("Force " + force);
            if (!Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, waterLayer)){
                aboveWater = true;
            }
           else {
                 aboveWater = false;
            }


            if(Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer) && aboveWater){
                  rigidbody.velocity = new Vector3();
                playerSM.ChangeState(playerSM.groundMoveState);
            }
            
          
            
            //calculate current speed of the player, also read player input
            currentSpeed = Vector3.Distance(prevPos, playerTransform.position)/Time.deltaTime;
            move = controls.WaterMove.Turn.ReadValue<Vector2>();
            throttle = controls.WaterMove.Accelerate.ReadValue<float>();
            xRotation = new Vector3(0,200 * move.x,0);
            rise = controls.WaterMove.Rise.ReadValue<float>();
            fall = controls.WaterMove.Fall.ReadValue<float>();
            jumpPresesed = (int)controls.WaterMove.Jump.ReadValue<float>();
            mouseMove = (controls.WaterMove.Rotate.ReadValue<Vector2>());

            //set the acceleration of the character based off its position in an animation curve
            float curveTime;
        
             if(controls.WaterMove.Boost.ReadValue<float>() < 1)  speedTarget = moveSpeed; else speedTarget = dashSpeed;
            if(throttle == 1){
                curveTime  = accelCurve.Evaluate(accelTime);
                speed = speedTarget * curveTime;
                if(accelTime < 1)accelTime += Time.deltaTime;
            }
            else{
                curveTime  = accelCurve.Evaluate(accelTime);
                speed = speedTarget * curveTime;
                if(accelTime > 0)accelTime -= Time.deltaTime;
            }

            if (rise + -fall > 0)  rising  = true;  else  rising  = false;
            if (rise + -fall < 0)  falling =  true; else  falling = false;
            if(aboveWater && force.y > 0)rising = true; else if(aboveWater && force.y < 0)falling = true;

            if(hit.distance <= 0.8f && currentSpeed > 200 && rising)jump = true;
            else if (hit.distance <= 0.8f && currentSpeed < 200 && !aboveWater){
                force.y = 0;
//                 Debug.Log(" distance from water line " + hit.distance + " current speed " + currentSpeed + " jump " + jump);
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                rising = false;
                rise = 0;
            }


             Debug.Log("Jump " + jump);

            if(controls.WaterMove.Boost.ReadValue<float>() > 0 && turbo > 0){
                boostEffectObj.SetActive(true);
            }
            else boostEffectObj.SetActive(false);         

            if(controls.WaterMove.Attack.ReadValue<float>() > 0){
                // animator.SetTrigger("Boost"); 
                publisher.updateBoostingStatus();
            }
        
            mouseMove = mouseMove.normalized;
            xRotation = new Vector3(0, 200 * move.x, 0);
            
            // animator.SetBool("Rising",  rising);
            publisher.updateRisingStatus(rising);
            // animator.SetBool("Falling", falling);
            publisher.updateFallingStatus(falling);
        }

        void PrintStateName(){}
        
        public void Exit(){
          //handle leaving player state
                Debug.Log("Moving to ground move state");

        }

    }

