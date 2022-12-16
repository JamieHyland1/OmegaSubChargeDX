using System.Security.AccessControl;
using System.Globalization;
using System.Threading;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


    public class PlayerSM : StateMachine
    {
        //====================================================================================================

        [Header("Speed variables")]

        [Tooltip("The force of gravity applied to the character when falling")]
        [Range(0, -150), SerializeField] private float gravity = -9.8f;
        [Range(1, 15), SerializeField] private int gravityScale = 1;

        [Tooltip("How fast the character falls when not holding the jump button")]
        [Range(1, 7), SerializeField] private float lowJumpMultiplier;

        [Tooltip("Move speed of the character when moving on a plane")]
        [Range(100, 500), SerializeField] private float moveSpeed;

        [SerializeField] float boostSpeed;
        [Tooltip("Move speed of the character when moving up or down")]
        [Range(100, 500),SerializeField] float ySpeed;
   


        //====================================================================================================

        [Header("Movement Vectors")]

        [Tooltip("Characters position on the next frame")]
        [SerializeField] private Vector3 nextPos;

        [Tooltip("Characters position on the previous frame")]
        [SerializeField] private Vector3 previousPos;

        [Tooltip("Move Direction of the character")]
        [SerializeField] private Vector3 moveDirection;

        [Tooltip("How much drag the character gets when dashing")]
        [SerializeField] private Vector3 Drag;

        [Tooltip("Speed of the character")]
        [SerializeField]  private Vector3 velocity; 
        [Tooltip("Speed of the character")]
        [SerializeField] private Vector3 prevVelocity;

        //====================================================================================================

        [Header("Movement checks")]

        [SerializeField] private bool walking = false;
        [SerializeField] private bool running = false;
        [SerializeField] private bool sliding = false;
        //reminder create getter and setter for isGrounded
        [SerializeField] public bool isGrounded = false;
        [SerializeField] private bool jumpButtonHeld = false;
        [SerializeField] private bool isDashing = false;
        [SerializeField] private bool coyoteJump = false;
        [SerializeField] private bool onSlope = false;

        //====================================================================================================

        [Header("Turn smooth time")]
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] float fallTriggerDeadzone;


        //====================================================================================================
        [Header("Layer Masks")]
        [SerializeField] public LayerMask waterLayer;


        //====================================================================================================    
        [Header("External References")]
       
        [SerializeField] private Transform cam;
        [SerializeField] public Transform groundCheck;
        [SerializeField] public Transform wallCheck;
        [SerializeField] private AnimationCurve accelCurve;
        [SerializeField] private AnimationCurve gravitylCurve;
        [SerializeField] ParticleSystem system;
        [SerializeField] Material attackMat;
        [SerializeField] Animator animator;
        [SerializeField] GameObject boostEffectObj;
        


        //debug
        [SerializeField] private Transform cameraPoint;


        //====================================================================================================
        Transform playerTransform;
        Rigidbody rigidbody;
        public PlayerControls controls;

        //====================================================================================================
        Vector2 move;

        //====================================================================================================
        float speed;
        float turnSmoothVelocity;
        float animationTimePosition;
        float slopeAngleThreshold = 44f;
        public float currentSpeed = 0;
        public Vector3 direction = new Vector2();
       

        enum States { moving };

        [SerializeField]
        States startingState;

        public MoveState moveState  { get; private set; }
     


        private void Awake(){
    
            controls = new PlayerControls();
            rigidbody = GetComponent<Rigidbody>();
            playerTransform = this.gameObject.transform;
            controls.Enable();
            moveState  = new MoveState(this, rigidbody, this.transform,  controls,  animator,   attackMat,  boostEffectObj, fallTriggerDeadzone, moveSpeed, ySpeed, boostSpeed, waterLayer, accelCurve);
        }

        private void Start(){
           this.ChangeState(moveState);
        }
      
        public float scale(float from, float to, float from2, float to2, float value){
        if(value <= from2){
            return from;
        }else if(value >= to2){
            return to;
        }else{
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }
    }



    public Vector3 getVelocity(){
        return velocity;
    }
    public void setVelocity(Vector3 vel){
        this.velocity = vel;
    }
    public Vector3 getDirection(){
        return this.direction;
    }
    public void setDirection(Vector3 dir){
        
        this.direction = dir;
    }

    public float getCurrentSpeed(){
        return this.currentSpeed;
    }
    public void setCurrentSpeed(float spd){
        this.currentSpeed = 0;
        this.currentSpeed = spd;
    }


  
    }

    


