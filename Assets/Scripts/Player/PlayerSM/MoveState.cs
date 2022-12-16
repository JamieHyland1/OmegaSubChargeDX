using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class MoveState : IState
{
    PlayerSM playerSM;
    PlayerControls controls;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float turnSmoothVelocity = 6000;
    [SerializeField]
    float turnSmoothTime = 0.3f;

    Transform playerTransform;
    [Range(0,1),SerializeField]
    public static float turbo = 1;
    [SerializeField]
    UIManager manager;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    GameObject boostEffectObj;
    [SerializeField]
    float fallTriggerDeadzone;
    [SerializeField]
    float dashSpeed;
  AnimationCurve accelCurve;

    Animator animator;
    Material attackMat;

    [SerializeField]
    int boostTime = 1;
    [SerializeField]
    bool inMenu = false;

    float throttle = 0;
    float rise;
    float fall;
    float dashTimer = 1.25f;
    bool isDashing = false;
    bool isBoosting = false;
    float counter = 0;
    float speed;
    float moveSpeed;
    float ySpeed;
    float accelTime;
    bool aboveWater = false;
    float currentSpeed = 0;
    
    Rigidbody rigidbody;
    Vector2 move;
    Vector2 mouseMove;
    Vector3 xRotation;
    Vector3 yRotation;
    Vector3 yVector;
    Vector3 prevPos;
    Vector3 moveDirection;
    float angle;
    float targetAngle;

    bool rising = false;
    bool falling = false;
      
        public MoveState(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform,  PlayerControls controls, Animator animator,  Material attackMat, GameObject boostEffectObj, float fallTriggerDeadzone, float moveSpeed, float ySpeed, float dashSpeed, LayerMask layerMask, AnimationCurve accelCurve){
            playerSM = _playerSM;
            this.rigidbody = rigidbody;
            this.playerTransform = playerTransform;
            this.cam = cam;
            this.controls = controls;
            this.animator = animator;
            this.attackMat = attackMat;
            this.boostEffectObj = boostEffectObj;
            this.fallTriggerDeadzone = fallTriggerDeadzone;
            this.moveSpeed = moveSpeed;
            this.ySpeed = ySpeed;
            this.dashSpeed = dashSpeed;
            this.layerMask = layerMask;
            this.accelCurve = accelCurve;
            Debug.Log("Move speed " + speed + " dash speed " + dashSpeed);
           
        }
        
        
        public void Enter(){
            yVector = new Vector3();
            move = new Vector2();   
            angle = 0;
            accelTime = 0;
        }


        public void FixedTick()
        {
            
        var camera = Camera.main;
        prevPos = playerTransform.position;
        // Y rotation based on camera
        float targetAngle = Mathf.Atan2(move.x, 0) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            
        
               
        if(!isDashing){
            
            rigidbody.MoveRotation(Quaternion.Euler(0,angle  * Time.timeScale,0));
            if(aboveWater)rigidbody.AddForce(Vector3.up * -ySpeed * 12 +  (playerTransform.forward * speed));
            else rigidbody.AddForce((Vector3.up * ySpeed * (rise + -fall)) +  (playerTransform.forward * speed * throttle));
            counter = dashTimer;

        }
        }

        public void Tick(){          
            RaycastHit hit;

            if (!Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask))
                aboveWater = true;
            else
                aboveWater = false;
           
            if(counter <= 0) isDashing = false;

            currentSpeed = Vector3.Distance(prevPos, playerTransform.position)/Time.deltaTime;
            move = controls.Move.Turn.ReadValue<Vector2>();
            throttle = controls.Move.Accelerate.ReadValue<float>();
            xRotation = new Vector3(0,200 * move.x,0);
         
            rise = controls.Move.Rise.ReadValue<float>();
            fall = controls.Move.Fall.ReadValue<float>();
            mouseMove = (controls.Move.Rotate.ReadValue<Vector2>());
            
            float curveTime;
          
            if(throttle == 1){
                curveTime  = accelCurve.Evaluate(accelTime);
                speed = moveSpeed * curveTime;
                if(accelTime < 1)accelTime += Time.deltaTime;
            }
            else{
                curveTime  = accelCurve.Evaluate(accelTime);
                speed = moveSpeed * curveTime;
                if(accelTime > 0)accelTime -= Time.deltaTime;
            }

            if (rise + -fall > 0)  rising  = true;  else  rising  = false;
            if (rise + -fall < 0)  falling =  true; else  falling = false;

            moveDirection = playerTransform.forward;

            if(hit.distance <= 0.5f && !isDashing){
                rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
            }
            
            yVector.y = (rise + -fall);

            if(controls.Move.Boost.ReadValue<float>() > 0 && turbo > 0){
                boostEffectObj.SetActive(true);
            }
            else boostEffectObj.SetActive(false);         

            if(controls.Move.Attack.ReadValue<float>() > 0){
                animator.SetTrigger("Boost"); 
            }
        
            mouseMove = mouseMove.normalized;
            moveDirection = moveDirection.normalized;
            
            if(!isDashing){
                xRotation = new Vector3(0, 200 * move.x, 0);
            }

            animator.SetBool("Rising",  rising);
            animator.SetBool("Falling", falling);

            counter -= Time.deltaTime;
        }

        void PrintStateName(){}
        
        public void Exit(){
          //handle leaving player state
        }
    }

