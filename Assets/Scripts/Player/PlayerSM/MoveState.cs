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
    Transform waterSurfaceCheck;
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
    
    

    
      
        public MoveState(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform, Transform groundCheck, Transform waterSurfaceCheck,  PlayerControls controls,  Material attackMat, GameObject boostEffectObj, float fallTriggerDeadzone, float moveSpeed, float ySpeed, float dashSpeed, AnimationCurve accelCurve, BoxCollider collider){
            playerSM = _playerSM;
            this.rigidbody = rigidbody;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.waterSurfaceCheck = waterSurfaceCheck;
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
            publisher = new PlayerEventPublisher();
            collider.center = new Vector3(0f,2.5f,-0.77f);
            collider.size = new Vector3(5f,5f,8.5f);
            
            waterLayer  = LayerMask.GetMask("Water");
            groundLayer = LayerMask.GetMask("Level Geometry");
            
            move = new Vector2();   
            angle = 0;
            
            camera = Camera.main;
            speed = moveSpeed;
            dashSpeed = moveSpeed * 1.5f;

            controls.WaterMove.Rise.performed += HandleRise;
            controls.WaterMove.Rise.canceled  += HandleRise;
            controls.WaterMove.Fall.performed += HandleFall;
            controls.WaterMove.Fall.canceled  += HandleFall;

            controls.WaterMove.Jump.performed += OnJump;
            controls.WaterMove.Jump.canceled += OnJump;

            timeToPeak = maxJumpTime/2;
            gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
            initialJumpVelocity = 2 * maxJumpHeight/timeToPeak;
            currentGravity = gravity;
        }


        public void FixedTick(){
            prevPos = playerTransform.position;
            // Y rotation based on camera
        

            rigidbody.MoveRotation(Quaternion.Euler(0,angle  * Time.timeScale,0));
        
            rigidbody.AddForce(force);

            ApplyGravity();
            HandleJump();

            force.x = 0;
            force.z = 0;
           
             if(submerged)force.y = 0;
             if(onSurface )rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
            Vector3 currentVelocity =  rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
            if(submerged)rigidbody.velocity = currentVelocity; else rigidbody.velocity = new Vector3(currentVelocity.x, rigidbody.velocity.y, currentVelocity.z);
            
         
        }

        public void Tick(){          
            //check if player is above the water line or not
            //TODO check if player is above water using a raycast pointing downward

            CheckSubmergedStatus();
            
            //calculate current speed of the player, also read player input
            currentSpeed = Vector3.Distance(prevPos, playerTransform.position)/Time.deltaTime;
            currentYSpeed = Vector3.Distance(new Vector3(0,prevPos.y,0), new Vector3(0, playerTransform.position.y, 0))/Time.deltaTime;
           
            
            move = controls.WaterMove.Move.ReadValue<Vector2>();
            throttle = controls.WaterMove.Accelerate.ReadValue<float>();
            mouseMove = (controls.WaterMove.Rotate.ReadValue<Vector2>());
            

            HandleRotation();

            //set the acceleration of the character based off its position in an animation curve
            
        
            if(controls.WaterMove.Boost.ReadValue<float>() < 1)  speed = moveSpeed; else speed = dashSpeed;
          

            if(controls.WaterMove.Boost.ReadValue<float>() > 0 && turbo > 0){
                boostEffectObj.SetActive(true);
            }
            else boostEffectObj.SetActive(false);         

            if(controls.WaterMove.Attack.ReadValue<float>() > 0){
                publisher.updateBoostingStatus();
            }
        
            mouseMove = mouseMove.normalized;
            xRotation = new Vector3(0, 200 * move.x, 0);
            
            publisher.updateRisingStatus(rising);
            publisher.updateFallingStatus(falling);
        }

        void HandleRise(InputAction.CallbackContext context){
            rising = context.ReadValueAsButton();
        }

        void HandleFall(InputAction.CallbackContext context){
            falling = context.ReadValueAsButton();
        }

        void HandleRotation(){
            if(move.magnitude > 0.1){
                
                targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.05f);
                Vector3 relativeForce = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            
                force.x = relativeForce.x * speed;
                force.z = relativeForce.z * speed;
               
            }
            if(rising && submerged && !onSurface && !aboveWater) force.y = ySpeed;
            else if(falling && submerged) force.y =  -ySpeed;
        }

        void CheckSubmergedStatus(){
            RaycastHit hit,hit2;
            RaycastHit groundHit;

            submerged = ((Physics.Raycast(playerTransform.position,  playerTransform.TransformDirection(Vector3.up),   out hit,  Mathf.Infinity, waterLayer)
                    &&   !Physics.Raycast(playerTransform.position,  playerTransform.TransformDirection(Vector3.down), out hit2, Mathf.Infinity, waterLayer)) 
                    ||   (Physics.OverlapSphere(playerTransform.position, 3.5f, waterLayer).Length > 0));

            
            distanceToSurface = hit.distance;
            onSurface = Physics.CheckSphere(waterSurfaceCheck.position, 0.1f , waterLayer);

            aboveWater = Physics.Raycast(playerTransform.position,  playerTransform.TransformDirection(Vector3.down), out hit2, Mathf.Infinity, waterLayer);

            if(Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer) && !submerged){
                rigidbody.velocity = new Vector3();
                publisher.updateTransformStatus();
                playerSM.ChangeState(playerSM.groundMoveState);
            }   
            
        }

        void ApplyGravity(){
            bool isFalling = ((rigidbody.velocity.y <= 0.0f) || !jumpPressed);
            float fallMultiplier = 7.0f;
            float previousYVelocity = force.y;
            float nextYvelocity = 0;


            if(isFalling){
                float newYVelocity = force.y + currentGravity * fallMultiplier;
                nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
                
            }else{
                float newYVelocity = force.y + currentGravity;
                nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            }

            if(!submerged)force.y += Mathf.Max(nextYvelocity,-300) * Time.fixedDeltaTime;
        }

        void HandleJump(){
            
            if(jumpPressed && onSurface &&  !jumping){
                rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
                jumping = true;
            }
            else if(!jumpPressed && submerged && jumping){
                jumping = false;
            }   
        }

        void OnJump(InputAction.CallbackContext context){
            jumpPressed = context.ReadValueAsButton();
        }

        void PrintStateName(){}

        public void Exit(){
        }

    }

