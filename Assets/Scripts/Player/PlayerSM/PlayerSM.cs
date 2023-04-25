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

    [Header("Speed variables Water")]
    [Tooltip("The force of gravity applied to the character when falling")]
    [Range(0, -150), SerializeField]
    private float gravity = -9.8f;

    [Range(1, 15), SerializeField] private int gravityScale = 1;

    [Tooltip("How fast the character falls when not holding the jump button")] [Range(1, 7), SerializeField]
    private float lowJumpMultiplier;

    [Tooltip("Move speed of the character when moving on a plane")] [Range(100, 1500), SerializeField]
    private float moveSpeed;

    [SerializeField] float boostSpeed;

    [Tooltip("Move speed of the character when moving up or down")] [Range(100, 500), SerializeField]
    float ySpeed;

    [Tooltip("The distance the player will dash")] [Range(0, 150), SerializeField]
    float dashDistance;



    //====================================================================================================

    [Header("Movement Vectors")] [Tooltip("Characters position on the next frame")] [SerializeField]
    private Vector3 nextPos;

    [Tooltip("Characters position on the previous frame")] [SerializeField]
    private Vector3 previousPos;

    [Tooltip("Move Direction of the character")] [SerializeField]
    private Vector3 moveDirection;

    [Tooltip("How much drag the character gets when dashing")] [SerializeField]
    private Vector3 Drag;

    [Tooltip("Speed of the character")] [SerializeField]
    private Vector3 velocity;

    [Tooltip("Speed of the character")] [SerializeField]
    private Vector3 prevVelocity;

    //====================================================================================================

    [Header("Turn smooth time")] [SerializeField]
    private float turnSmoothTime = 0.1f;

    [SerializeField] float fallTriggerDeadzone;


    //====================================================================================================
    [Header("Layer Masks")] [SerializeField]
    public LayerMask waterLayer;


    //====================================================================================================    
    [Header("External References")] [SerializeField]
    private Transform cam;

    [SerializeField] public Transform groundCheck;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public Transform ledgeCheck;
    [SerializeField] public Transform waterSurfaceCheck;
    [SerializeField] public Transform slopeCheck;
    [SerializeField] private AnimationCurve accelCurve;
    [SerializeField] private AnimationCurve gravitylCurve;
    [SerializeField] private AnimationCurve dashCurve;
    [SerializeField] ParticleSystem system;
    [SerializeField] Material attackMat;
    [SerializeField] GameObject boostEffectObj;
    [SerializeField] GameObject torpedoObj;



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
    public CapsuleCollider collider;
    private LockOn _lockOn;

    public Vector3 storedVelocty { get; set; }
    public Vector3 storedForce { get; set; }
    public bool jumpDash { get; set; }


    enum States
    {
        WaterMove,
        GroundMove
    };

    [SerializeField] States startingState;
    public WaterMoveState _WaterMoveState { get; private set; }
    public GroundMoveState _GroundMoveState { get; private set; }
    public DashState _DshState { get; private set; }
    public DashJump  _DashJump { get; private set; }
    public LedgeGrabState _LedgeGrabState { get; private set; }



private void Awake(){
            controls = new PlayerControls();
            rigidbody = GetComponent<Rigidbody>();
            _lockOn = GetComponent<LockOn>();
            playerTransform = this.gameObject.transform;
            controls.Enable();
            _WaterMoveState   = new WaterMoveState(this, rigidbody, this.transform, groundCheck, waterSurfaceCheck,  controls,   attackMat,  boostEffectObj,torpedoObj, fallTriggerDeadzone, moveSpeed, ySpeed, boostSpeed, accelCurve, collider,_lockOn);
            _GroundMoveState  = new GroundMoveState(this, rigidbody, controls, this.transform, groundCheck, wallCheck, ledgeCheck, slopeCheck, collider);
            _DshState         = new DashState(this, rigidbody, this.transform, groundCheck, waterSurfaceCheck, wallCheck, controls, dashCurve, dashDistance);
            _DashJump         = new DashJump(this, rigidbody, this.transform, groundCheck, waterSurfaceCheck, controls);
            _LedgeGrabState   = new LedgeGrabState(this, rigidbody,this.transform,wallCheck,ledgeCheck,controls);
}

        private void Start(){
            switch(startingState){
                case States.GroundMove:
                    this.ChangeState(_GroundMoveState);
                    break;
                case States.WaterMove:
                    this.ChangeState(_WaterMoveState);
                    break;
            }
           
        }

        // public void Update()
        // {
        //     Debug.Log("transform " + this.transform.forward);
        // }
        //

    // void OnDrawGizmos(){
    //     // Draw a yellow sphere at the transform's position
    //     // RaycastHit wallHit;
    //     // LayerMask groundLayer = LayerMask.GetMask("Level Geometry");
    //     // Gizmos.color = Color.yellow;
    //     // if(Physics.SphereCast(wallCheck.position,2.5f,this.transform.forward,out wallHit, groundLayer))Debug.DrawRay(wallCheck.position,transform.forward * 5,Color.magenta,1.0f);
    //     // Gizmos.DrawWireSphere(this.transform.position + Vector3.up * 6.6f, 80);
    //     // RaycastHit hit;
    //     // Physics.Raycast(groundCheck.position + Vector3.up, this.transform.TransformDirection(Vector3.forward), out hit, 5, groundLayer);
    //     // // Gizmos.DrawSphere(hit.point,1);
    //     //  Gizmos.DrawLine(hit.point, hit.point + hit.normal * 6);
    //    
    // }
  
    }

    


