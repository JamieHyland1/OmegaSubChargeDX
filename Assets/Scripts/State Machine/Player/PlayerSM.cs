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

        [Tooltip("Move speed of the character when walking")]
        [Range(0, 155), SerializeField] private float moveSpeed;

        [Tooltip("Move speed of the character when running")]
        [Range(0, 160), SerializeField] private float runSpeed;

        [Tooltip("Move speed of the character when sliding")]
        [Range(5, 160), SerializeField] private float slideSpeed;

        [Tooltip("How fast the character jumps up in the air")]
        [Range(1, 155), SerializeField] private float jumpVelocity;

        [Tooltip("How far the character dashes")]
        [Range(5, 100), SerializeField] private float dashDistance;

        [Tooltip("How much drag is applied to the player when sliding")]
        [Range(0, 1), SerializeField] private float dragCoeffecient;
        [Tooltip("How far the system checks for slopes")]
        [Range(1, 20), SerializeField] private float slopeCheckLenght;


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


        //====================================================================================================
        [Header("Layer Masks")]
        [SerializeField] public LayerMask groundLayer;


        //====================================================================================================    
        [Header("External References")]
       
        [SerializeField] private Transform cam;
        [SerializeField] public Transform groundCheck;
        [SerializeField] private Transform slopeCheck;
        [SerializeField] public Transform wallCheck;
        [SerializeField] private AnimationCurve dashCurve;
        [SerializeField] private AnimationCurve accelCurve;
        [SerializeField] private AnimationCurve gravitylCurve;
        [SerializeField] public ParticleSystem system;
        


        //debug
        [SerializeField] private Transform cameraPoint;


        //====================================================================================================
        Transform playerTransform;
        CharacterController controller;
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
        PhysicsHelper helper;
       

        enum States { idle, moving, jumping, dashing, sliding };

        [SerializeField]
        States startingState;

        public MoveState moveState  { get; private set; }
        public AirMoveState airMoveState  { get; private set; }
        public RunState runState    { get; private set; }
        public DashState dashState  { get; private set; }
        public WallSlideState wallSlide  { get; private set; }
        public SlidingState slideState  { get; private set; }


        private void Awake(){
            controller = this.GetComponent<CharacterController>();
            controls = new PlayerControls();
            playerTransform = this.gameObject.transform;
//            EventManager.current.OnPlayerEnter+=PlayerDashRingContact;
         
            controls.Enable();
            helper = PhysicsHelper.Instance;
            helper.gravity = gravity;
            helper.gravityScale = gravityScale;
            helper.moveSpeed = moveSpeed;
            helper.runSpeed = runSpeed;
            helper.afterDashSpeed = 15;
            helper.jumpVelocity = jumpVelocity;
            wallSlide  = new WallSlideState(this, playerTransform, groundCheck, wallCheck, controls,  ref controller, groundLayer, helper);
            moveState  = new MoveState(this,  playerTransform,  cam,  accelCurve, controls, ref controller, ref move,  moveSpeed,  runSpeed,  turnSmoothVelocity,  turnSmoothTime, groundLayer, helper );
            airMoveState  = new AirMoveState(this,  playerTransform,  cam, wallCheck,  accelCurve, controls, ref controller, ref move,  moveSpeed,  runSpeed,  turnSmoothVelocity,  turnSmoothTime, groundLayer, helper );
            dashState  = new DashState(this, ref controller, dashCurve,playerTransform, runSpeed*4, dashDistance, helper);
            slideState = new SlidingState(this, playerTransform,groundCheck, cam, ref controller, controls,   0.4f, 0.3f);
   
           
            // controls.Gameplay.Slide.started += ctx =>  this.ChangeState(this.slideState);
            // controls.Gameplay.Slide.canceled += ctx => this.ChangeState(this.moveState);
           

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

        // Hit scan an area of 180 degrees in front of the player to check for a steep slope
        // Calculate the angle of the of the slope to the player and if its more than our allowed slope climb return false
        public bool checkForSlopes(){
            for(float i = -90 * Mathf.Deg2Rad; i <= 90 * Mathf.Deg2Rad; i+=0.1f){ 
                slopeCheck.rotation = transform.rotation * Quaternion.Euler(0f, i * Mathf.Rad2Deg, 0f);
                Ray ray = new Ray(slopeCheck.position, new Vector3(slopeCheck.forward.x,0,slopeCheck.forward.z));
                RaycastHit hit;
                Physics.Raycast(ray, out hit, slopeCheckLenght);
                if(hit.collider != null){
                    float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up,hit.normal);
                    Debug.Log("slope " + slopeAngle * Mathf.Rad2Deg);
                    float radius = Mathf.Abs(1/Mathf.Sin(slopeAngle));
                    if(slopeAngle >= slopeAngleThreshold * Mathf.Deg2Rad){
                        if(hit.distance - controller.radius > Mathf.Abs(Mathf.Cos(slopeAngle) * radius)){
                            onSlope = false;
                            return true;
                        }
                        onSlope = true;
                        return false;
                    }
                    onSlope = false;
                    return true;
                }
            }
            onSlope = false;
            return true;
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


        // Calculate the force of the player moving down a slope
        // Uses the normal of the slope and calculates a force to apply to the players velocity vector
        public Vector3 slideDownSlope(Vector3 velocity){
            for(float i = -90 * Mathf.Deg2Rad; i <= 90 * Mathf.Deg2Rad; i+=0.1f){
                slopeCheck.rotation = transform.rotation * Quaternion.Euler(0f,i * Mathf.Rad2Deg, 0f);
                Ray ray = new Ray(slopeCheck.position, new Vector3(slopeCheck.forward.x,0,slopeCheck.forward.z) * 6.5f);
                RaycastHit hit;
                Physics.Raycast(ray, out hit, slopeCheckLenght, groundLayer);
                if(hit.collider != null){
                    velocity.x = (2f - hit.normal.y) * hit.normal.x * (-gravity - 0.005f);
                    velocity.z = (2f - hit.normal.y) * hit.normal.z * (-gravity - 0.005f);
                    return velocity;
                }
            }
            Debug.Log("Slope vel " + velocity);
            return velocity*-1;
        }   

        void OnDrawGizmos(){
            if(Application.isPlaying){
                Gizmos.DrawSphere(wallCheck.position,1f);
            }    
        }


        private void PlayerDashRingContact(Vector3 ringPos, Vector3 ringDir){
        if(this.currentState == dashState){
            this.transform.position = ringPos; 
            this.transform.forward = ringDir;
            this.currentState.EventTrigger();
        }else{
            this.transform.position = ringPos; 
            this.transform.forward = ringDir;
            this.ChangeState(dashState);
        }
    }
    }

    


