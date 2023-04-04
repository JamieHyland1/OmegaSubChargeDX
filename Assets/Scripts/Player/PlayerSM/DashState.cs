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
    bool jumping;
    private bool isGrounded;
    
    
    int boostTime = 1;
    private float previousDistanceToTarget;
    float gravity;
    float animationTimePosition;
    float dashDistance;
    private float groundedGravity = 0.85f;
    private bool _jumpPressed;


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
            publisher = new PlayerEventPublisher();
           
            publisher.updateStateChange("Mech dash");
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
            
            rigidbody.isKinematic = true;
            animationTimePosition = 0;
          
       
            controls.Enable();
            controls.GroundMove.DashJump.performed += OnJump;
            // controls.GroundMove.Jump.canceled += OnJump;
        }
        public void FixedTick(){
            Vector3 t = Vector3.Lerp(playerTransform.position, dashLocation, dashCurve.Evaluate(animationTimePosition));
            rigidbody.MovePosition(Vector3.Lerp(playerTransform.position, dashLocation, dashCurve.Evaluate(animationTimePosition*2)));
            HandleJump();
        }
       
        public void Tick()
        {
            if (playerTransform.parent != null) playerTransform.parent = null;

            float d = Vector3.Distance(playerTransform.position, dashLocation);
             if(d > 0.6f)
             {
                 publisher.playDashEvent();
                 
                float t = Time.deltaTime*2;
                animationTimePosition += t;
            
            }else
             {
                 playerTransform.position = dashLocation;
                 playerSM.ChangeState(playerSM._GroundMoveState);
            }
             publisher.updateForce(force);
             publisher.updateVelocity(rigidbody.velocity);
             publisher.updateSubmerged(Physics.CheckSphere(groundCheck.position, 0.25f , waterLayer));
        }
        void OnJump(InputAction.CallbackContext context){
                Debug.Log("Dash state jump");
                _jumpPressed = context.performed;
                playerTransform.parent = null;
                
                if (_jumpPressed )
                {
                    publisher.updateJumpedStatus();
                }
        }
        void HandleJump()
        {
            if (_jumpPressed )
            {
                rigidbody.isKinematic = false;
                _jumpPressed = false;
                // rigidbody.AddForce(Vector3.up * 4,ForceMode.Impulse);
                playerSM.ChangeState(playerSM._DashJump);
            }
        }
        void PrintStateName()
        {
        }
        
        public void Exit()
        {
            if (_jumpPressed)
            {
                playerSM.storedVelocty = rigidbody.velocity;
                playerSM.jumpDash = true;
            }

            rigidbody.isKinematic = false;
            controls.GroundMove.DashJump.performed -= OnJump;
        }

}