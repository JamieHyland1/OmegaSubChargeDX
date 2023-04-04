using UnityEngine.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WaterMoveState : IState
{
    PlayerSM playerSM;
    PlayerControls controls;
    Transform playerTransform;
    Transform groundCheck;
    Transform waterSurfaceCheck;
    LayerMask waterLayer;
    LayerMask groundLayer;
    GameObject boostEffectObj;
    private GameObject torpedo;
    AnimationCurve accelCurve;
    Material attackMat;
    Rigidbody rigidbody;
    Camera camera;
    PlayerEventPublisher publisher;
    CapsuleCollider collider;
    private LockOn _lockOn;

    Vector2 move;
    Vector2 mouseMove;
    Vector3 xRotation;
    Vector3 yRotation;
    Vector3 prevPos;
    Vector3 force;
   
    float rise;
    float fall;
    float dashTimer = 1.25f;
    float counter = 0;
    float speed;
    float moveSpeed;
    float ySpeed;
    float accelTime;
    float dashSpeed;
    float jumpForce = 100;
    float speedTarget;
    float turnSmoothVelocity = 0;
    float turnSmoothTime = 0.3f;
    private float rotationSpeed = 1755;
    float angle;
    float targetAngle;
    float dragForce = 5.5f;
    float distanceToSurface;

   
    bool aboveWater = false;
    bool isBoosting = false;
    bool inMenu = false;
    bool rising = false;
    bool falling = false;
    bool jump = false;
    bool submerged;
    bool jumpPressed;
    bool jumping;
    bool lockOnPressed;

    int boostTime = 1;
    float throttle;
  
    
    

    
      
        public WaterMoveState(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform, Transform groundCheck,
            Transform waterSurfaceCheck, PlayerControls controls, Material attackMat, GameObject torpedo,
            GameObject boostEffectObj, float fallTriggerDeadzone, float moveSpeed, float ySpeed, float dashSpeed,
            AnimationCurve accelCurve, CapsuleCollider collider, LockOn _lockOn){
            playerSM = _playerSM;
            this.rigidbody = rigidbody;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.waterSurfaceCheck = waterSurfaceCheck;
            this.controls = controls;
            this.attackMat = attackMat;
            this.boostEffectObj = boostEffectObj;
            this.torpedo = torpedo;
            this.moveSpeed = moveSpeed;
            this.dashSpeed = dashSpeed;
            this.accelCurve = accelCurve;
            this.collider = collider;
            this._lockOn = _lockOn;
        }
        
        public void Enter(){
            publisher = new PlayerEventPublisher();
            publisher.updateStateChange("Submarine");
            publisher.changeToSubCamera();
            controls.Enable();
            GeneralEventHandler.onPlayerEnterDashRing += HandleDashRing;
            collider.center = new Vector3(0,1.5f,-0.7f);
            collider.radius = 1.64f;
            collider.height = 7.83f;
            collider.direction = 2;
            waterLayer  = LayerMask.GetMask("Water");
            groundLayer = LayerMask.GetMask("Level Geometry");
            rigidbody.drag = 5f;
            
            move = new Vector2();   
            angle = 0;
          
            camera = Camera.main;
            speed = moveSpeed;
            dashSpeed = moveSpeed * 1.5f;

            controls.WaterMove.Rise.performed   += HandleRise;
            controls.WaterMove.Rise.canceled    += HandleRise;
            controls.WaterMove.Fall.performed   += HandleFall;
            controls.WaterMove.Fall.canceled    += HandleFall;
            controls.WaterMove.Lockon.performed += HandleLockOn;
            controls.WaterMove.Lockon.canceled  += HandleLockOn;
            controls.WaterMove.Attack.performed += OnAttack;
            controls.WaterMove.Attack.canceled  += OnAttack;
            controls.WaterMove.Throttle.performed += HandleThrottle;
            controls.WaterMove.Throttle.canceled += HandleThrottle;

        }


        public void FixedTick(){
            if (!lockOnPressed){
                ApplyRotation();
            }
           rigidbody.AddForce(force);
            ApplyGravity();
            // if (rigidbody.velocity.magnitude > speed)
            // {
            //     rigidbody.velocity = rigidbody.velocity.normalized * speed;
            // }
           
            if(submerged)force.y *= 0.75f;
        }

        public void Tick(){          

            CheckSubmergedStatus();
            
            
            move = controls.WaterMove.Move.ReadValue<Vector2>();
            mouseMove = (controls.WaterMove.Rotate.ReadValue<Vector2>());
            

            HandleRotation();

            if(controls.WaterMove.Boost.ReadValue<float>() < 1)  speed = moveSpeed; else speed = dashSpeed;
          
    

            if(controls.WaterMove.Attack.ReadValue<float>() > 0){
                publisher.updateBoostingStatus();
            }
        
            mouseMove = mouseMove.normalized;
            xRotation = new Vector3(0, 200 * move.x, 0);
     
            publisher.updateForce(force);
            publisher.updateVelocity(rigidbody.velocity);
            publisher.updateSubmerged(Physics.CheckSphere(groundCheck.position, 0.25f , waterLayer));
        }

        void HandleRise(InputAction.CallbackContext context){
            rising = context.ReadValueAsButton();
        }

        void HandleFall(InputAction.CallbackContext context){
            falling = context.ReadValueAsButton();
        }

        void HandleRotation(){
            if(move.magnitude > 0.1 && lockOnPressed && submerged){
                targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
                angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.05f);
                Vector3 relativeForce = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            
                force.x = relativeForce.x * speed;
                force.z = relativeForce.z * speed;
                if (Mathf.Abs(force.x) > speed) force.x = speed * Mathf.Sign(force.x);
                if (Mathf.Abs(force.z) > speed) force.z = speed * Mathf.Sign(force.z);
            }
            else if (!lockOnPressed && submerged) {
                Vector3 currentForce = playerTransform.forward * (speed * throttle);
                force = currentForce;
                if (currentForce.magnitude > speed) force = currentForce.normalized * speed;
            }
        }

        void CheckSubmergedStatus(){
            RaycastHit groundHit;
            submerged = (Physics.OverlapSphere(playerTransform.position, 0.5f, waterLayer).Length > 0);    

            if(Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer) && !submerged){
                publisher.updateTransformStatus();
                playerSM.ChangeState(playerSM._GroundMoveState);
            }   
            
        }

        void ApplyGravity(){
            // bool isFalling = ((rigidbody.velocity.y <= 0.0f) || !jumpPressed);
            // float fallMultiplier = 7.0f;
            // float previousYVelocity = force.y;
            // float nextYvelocity = 0;
            //
            //
            // if(isFalling){
            //     float newYVelocity = force.y + currentGravity * fallMultiplier;
            //     nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            //     
            // }else{
            //     float newYVelocity = force.y + currentGravity;
            //     nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            // }
            //
            // if (!submerged) force.y += -9.8f * 8;//Mathf.Max(nextYvelocity,-300) * Time.fixedDeltaTime;
            // Debug.Log("force " + force);
             if (!submerged) force.y += -9.8f * 4.5f;
        }

        void ApplyRotation()
        {
            
            Vector3 camRot = camera.transform.eulerAngles;
            camRot = new Vector3(playerTransform.localRotation.eulerAngles.x + move.y * rotationSpeed * Time.fixedDeltaTime, playerTransform.localRotation.eulerAngles.y + move.x * rotationSpeed * Time.fixedDeltaTime,0);
            rigidbody.transform.localRotation = Quaternion.Slerp(playerTransform.localRotation,Quaternion.Euler(camRot), 4.8f * Time.fixedDeltaTime);
          
        }

        void PrintStateName(){}
        
        void HandleLockOn(InputAction.CallbackContext context)
        {
            if (context.performed && _lockOn.GetTarget() != null)
            {
                lockOnPressed = !lockOnPressed;
            }
            publisher.targetEnemy(lockOnPressed);
       
        }

        void HandleThrottle(InputAction.CallbackContext context)
        {
             Debug.Log("Throttle " + context.ReadValue<float>());
            throttle = context.ReadValue<float>();
        }

        void OnAttack(InputAction.CallbackContext context)
        {
            Debug.Log("Attack " + context.performed);
            if (context.performed)
            {
                GameObject gameObject = MonoBehaviour.Instantiate(boostEffectObj,playerTransform.position,(Quaternion.identity));
                gameObject.transform.rotation = playerTransform.rotation;
                ParticleSystem system = gameObject.GetComponent<ParticleSystem>();
               //  
               //  gameObject.transform.forward = -playerTransform.forward;
               //  gameObject.GetComponent<Rigidbody>().drag = 5;
               // // gameObject.GetComponent<Rigidbody>().velocity = rigidbody.velocity;
                if (_lockOn.GetTarget() != null)
                {
                    
                    // gameObject.GetComponent<Missile>().SetTarget(_lockOn.GetTarget());
                }
                else
                {
                    
                  
                    ParticleSystem.ShapeModule shape = system.shape;
                    shape.rotation = new Vector3(90, -90, 0);
                    // shape.position = new Vector3();
                    // shape.angle = 1.41f;
                    // shape.radius = 1.77f;
                    // ParticleSystem.VelocityOverLifetimeModule vel = system.velocityOverLifetime;
                    // vel.radial = 40;
                    gameObject.SetActive(true);
                }

            }
        }


        public void Exit(){
             GeneralEventHandler.onPlayerEnterDashRing -= HandleDashRing;
             controls.WaterMove.Rise.performed -= HandleRise;
             controls.WaterMove.Rise.canceled  -= HandleRise;
             controls.WaterMove.Fall.performed -= HandleFall;
             controls.WaterMove.Fall.canceled  -= HandleFall;
             rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
             controls.Disable();
        }

        public void HandleDashRing( Vector3 direction){
            Debug.Log("Dash");
            playerTransform.forward = (direction);
            rigidbody.AddForce(direction * 250, ForceMode.Impulse);
        }

    }

