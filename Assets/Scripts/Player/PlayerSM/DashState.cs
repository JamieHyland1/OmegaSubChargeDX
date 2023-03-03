using UnityEngine.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DashState : IState{

     PlayerSM playerSM;
    PlayerControls controls;
    Transform playerTransform;
    Transform groundCheck;
    Transform waterSurfaceCheck;
    Transform wallCheck;
    LayerMask waterLayer;
    LayerMask groundLayer;
    GameObject boostEffectObj;
    AnimationCurve accelCurve;
    Material attackMat;
    Rigidbody rigidbody;
    Camera camera;
    PlayerEventPublisher publisher;
    CapsuleCollider collider;
    AnimationCurve dashCurve;

    Vector2 move;
    Vector3 prevPos;
    Vector3 xRotation;
    Vector3 yRotation;
    Vector3 nextPos;
    Vector3 force;
    Vector3 dashLocation;
   
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
    float currentYSpeed;
    float jumpForce = 100;
    float speedTarget;
    float turnSmoothVelocity = 0;
    float turnSmoothTime = 0.3f;
    public static float turbo = 1;
    float angle;
    float targetAngle;
    float dragForce = 5.5f;
    float distanceToSurface;

    // bool isDashing = false;
    bool aboveWater = false;
    bool isBoosting = false;
    bool inMenu = false;
    bool rising = false;
    bool falling = false;
    bool jump = false;
    bool submerged;
    bool jumpPressed;
    bool onSurface;
    bool jumping;
    
    int boostTime = 1;
    int jumpPresesed = 0;
    float timeToPeak;
    float maxJumpHeight = 15;
    float maxJumpTime = .5f;
    float gravity;
    float currentGravity;
    float initialJumpVelocity;
    float animationTimePosition;
    float dashDistance;


     public DashState(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform, Transform groundCheck, Transform waterSurfaceCheck, Transform wallCheck, PlayerControls controls, AnimationCurve dashCurve, float dashDistance){
            this.playerSM = _playerSM;
            this.rigidbody = rigidbody;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.waterSurfaceCheck = waterSurfaceCheck;
            this.wallCheck = wallCheck;
            this.controls = controls;
            this.dashCurve = dashCurve;
            this.dashDistance = dashDistance;
        }
        
        public void Enter(){
            Debug.Log("Dash State");
            publisher = new PlayerEventPublisher();
            controls = new PlayerControls();
            animationTimePosition = 0;
            groundLayer = LayerMask.GetMask("Level Geometry");
            waterLayer  = LayerMask.GetMask("Water");
            nextPos = playerTransform.position;
            Ray ray = new Ray(playerTransform.position,playerTransform.forward );
            RaycastHit hit;
            Physics.Raycast(ray,  out hit, dashDistance);
            if(hit.collider != null){
                dashLocation = new Vector3(hit.point.x ,hit.point.y,hit.point.z - 2 - 0.2f);
            }else{
                dashLocation = playerTransform.position + playerTransform.forward * dashDistance;
            }
            Debug.Log("Dash Location " + dashLocation);
        }


        public void FixedTick(){
       
         
        }

        public void Tick(){       
            
             Debug.Log("distance to target " + Vector3.Distance(playerTransform.position, dashLocation)); 
             if(Vector3.Distance(playerTransform.position, dashLocation) > 0.1f)
             {
                prevPos = playerTransform.position; 
                float amount = dashCurve.Evaluate(animationTimePosition);
                nextPos = Vector3.Lerp(playerTransform.position,dashLocation,(amount));
                if(Physics.CheckSphere(wallCheck.position + nextPos, 0.5f ,groundLayer))playerSM.ChangeState(playerSM._GroundMoveState);
                playerTransform.position += ((nextPos-playerTransform.position) * (125 * Time.deltaTime));
                publisher.playDashEvent();
                float t = Time.deltaTime;
                animationTimePosition += t;

            }else{
              playerSM.ChangeState(playerSM._GroundMoveState);
            }
        }

  

        void PrintStateName(){}

        public void Exit(){
           
        }

       

}