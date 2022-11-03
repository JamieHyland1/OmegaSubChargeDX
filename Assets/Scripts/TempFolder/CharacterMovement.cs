
// using System;
// using System.Collections;
// using UnityEngine;
// using UnityEngine.InputSystem;


// [RequireComponent(typeof(CharacterController))]
// public class CharacterMovement : MonoBehaviour
// {
// //====================================================================================================

//     [Header("Speed variables")]
    
//     [Tooltip("The force of gravity applied to the character when falling")]
//     [Range(0,-150),SerializeField] private float gravity = -9.8f;
//     [Range(1,15),SerializeField] private int gravityScale = 1;

//     [Tooltip("How fast the character falls when not holding the jump button")]
//     [Range(1,7),  SerializeField] private float lowJumpMultiplier;
    
//     [Tooltip("Move speed of the character when walking")]
//     [Range(1,155), SerializeField] private float moveSpeed;
  
//     [Tooltip("Move speed of the character when running")]
//     [Range(5,160), SerializeField] private float runSpeed;

//     [Tooltip("Move speed of the character when sliding")]
//     [Range(5,160), SerializeField] private float slideSpeed;
  
//     [Tooltip("How fast the character jumps up in the air")]
//     [Range(1,155), SerializeField] private float jumpVelocity;
  
//     [Tooltip("How far the character dashes")]
//     [Range(5,100),  SerializeField] private float DashDistance;

//     [Tooltip("How much drag is applied to the player when sliding")]
//     [Range(0,1),  SerializeField] private float dragCoeffecient;

// //====================================================================================================

//     [Header("Movement Vectors")]
    
//     [Tooltip("Characters position on the next frame")]
//     [SerializeField] private Vector3 nextPos; 
    
//     [Tooltip("Characters position on the previous frame")]
//     [SerializeField] private Vector3 previousPos; 
    
//     [Tooltip("Move Direction of the character")]
//     [SerializeField] private Vector3 moveDirection;

//     [Tooltip("How much drag the character gets when dashing")]
//     [SerializeField] private Vector3 Drag;
    
//     [Tooltip("Speed of the character")]
//     [SerializeField] private Vector3 velocity;
//     [Tooltip("Speed of the character")]
//     [SerializeField] private Vector3 prevVelocity;

// //====================================================================================================

//     [Header("Movement checks")]

//     [SerializeField] private bool walking = false;
//     [SerializeField] private bool running = false;
//     [SerializeField] private bool jumping = false;
//     [SerializeField] private bool sliding = false;
//     [SerializeField] private bool isGrounded = true;
//     [SerializeField] private bool jumpButtonHeld = false;
//     [SerializeField] private bool isDashing = false;
//     [SerializeField] private bool coyoteJump = false;

// //====================================================================================================
  
//     [Header("Turn smooth time")]
//     [SerializeField] private float turnSmoothTime = 0.1f;


// //====================================================================================================
//     [Header("Layer Masks")]
//     [SerializeField] private LayerMask groundLayer;


// //====================================================================================================    
//     [Header("External References")]
//     [SerializeField] private Animator animator;
//     [SerializeField] private Transform cam;
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private AnimationCurve dashCurve;
    
//     //debug
//     [SerializeField] private Transform cameraPoint;
    

// //====================================================================================================
//     Transform playerTransform;
//     CharacterController controller;
//     PlayerControls controls;

// //====================================================================================================
//     Vector3 direction;
//     Vector2 move;

// //====================================================================================================
//     float speed;
//     float turnSmoothVelocity;
//     float animationTimePosition;
//     //debug get rid of later once dash is finished
//     //Vector3 nextPos;
    

//     void Awake(){
        
//         controller = this.GetComponent<CharacterController>();
        
//         velocity = new Vector3();
        
//         playerTransform = this.transform;

//         controls = new PlayerControls();
        
//         controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
//         controls.Gameplay.Move.canceled  += ctx => move = Vector2.zero;
//         controls.Gameplay.Jump.started += ctx => Jump();
//         controls.Gameplay.Jump.canceled += ctx => jumpButtonHeld = false;
//         controls.Gameplay.Slide.started += ctx => sliding = true;
//         controls.Gameplay.Slide.canceled += ctx => sliding = false;
//         controls.Gameplay.Dash.performed += ctx => StartCoroutine("Dash");
//     }

    
//     void Start(){
//        speed = 0;
//     }


//     void Update()
//     {
//         checkGroundCollision();
//         if (sliding) Slide();
//         if (!sliding)
//         {
//             velocity.x = 0;
//             velocity.z = 0;

//         }

//         direction = new Vector3(move.x, 0f, move.y);

//         float xDir = scale(-1, 1, -2, 2, direction.x);
//         float zDir = scale(-1, 1, -2, 2, direction.z);
//       //  Debug.Log(xDir + " " + zDir);
//         animator.SetFloat("Velocity X", xDir);
//         animator.SetFloat("Velocity Z", zDir);
//         animator.SetBool("IsDashing", isDashing);
//         speed = Mathf.Lerp(moveSpeed, runSpeed, direction.magnitude);

//         if (!isGrounded && !isDashing) {
//             velocity.y += gravity * Time.deltaTime;
//             jumping = true;
//         }

//         if (direction.magnitude != 0 && !isDashing && !sliding) { 

//             animator.SetBool("IsRunning", running);
//             animator.SetBool("IsWalking", walking);
            

//             float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
//             float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

//             transform.rotation = Quaternion.Euler(0, angle, 0);
//             if (!sliding)
//                 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized * speed;
//             else
//                 moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

//             velocity.x += moveDirection.x;
//             velocity.z += moveDirection.z;


//         } else
//             animator.SetBool("IsWalking", !walking);

//         //Debug.Log(velocity);

//         if (!isDashing)
//         {
//             if (!isGrounded && velocity.y > 0 && !jumpButtonHeld) velocity.y += (gravity * gravityScale) * Time.deltaTime;
//             else if (!isGrounded && jumpButtonHeld) velocity.y += (gravity) * Time.deltaTime;
//             else if (isGrounded && velocity.y < 0) velocity.y = 0;
//         }
//         animator.SetBool("IsJumping", jumping);
//         animator.SetBool("IsSliding", sliding);


//         //Extrapolate the current position n frame to see if the player is touching the ground the next frame
//         //this allows me to check if the user pressed jump n frames before the player hit the ground, and to store
//         //that input to be run when the player does hit the ground
//         nextPos = new Vector3();
//         nextPos = transform.position;

//         // nextPos.x =  playerTransform.position.x + moveDirection.x * speed * Time.deltaTime;
//         // nextPos.y =  playerTransform.position.y + velocity.y;
//         // nextPos.z =  playerTransform.position.z + velocity.z * speed * Time.deltaTime;

//         if (!sliding || !isDashing)
//         {
//             controller.Move((velocity) * Time.deltaTime);
//             prevVelocity = velocity;
//         // Drag = Vector3.
//             //reset ground velocity
          
//         }
//         //Debug.Log(playerTransform.position + playerTransform.forward * DashDistance);
       
//     }   


  

//     public void Jump(){
//         jumping = false;
//         if(isGrounded && !jumping){
//        //   Debug.Log("JAMPU");

//             velocity.y = Mathf.Sqrt(jumpVelocity*-2f*gravity * lowJumpMultiplier);
//             jumping = true;
//         }
//         jumpButtonHeld = true;
//     }
//     //Need to work on not overriding drag value
//     public void Slide(){
       
//     }


//     public void checkGroundCollision(){
//         isGrounded = Physics.CheckSphere(groundCheck.position,0.1f,groundLayer,QueryTriggerInteraction.Ignore);
//         animator.SetBool("IsGrounded", isGrounded);
//         if(isGrounded == true && jumping == true) jumping = false;
//     }

//     void OnEnable(){
//         controls.Gameplay.Enable();
//     }
//     void OnDisable(){
//         controls.Gameplay.Disable();
//     }


// //NEED TO REFACTOR THIS FUNCTION OUT OF CLASS EVENTUALLY
//     public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
//         float OldRange = (OldMax - OldMin);
//         float NewRange = (NewMax - NewMin);
//         float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
 
//         return(NewValue);
//     }
//     // TODO: create a copy of current velocity
//     // disable gravity
//     // move horizontally at dash speed
//     // reapply velocity before dash
//     // void Dash(){
//     //      // Debug.Log("DASHU");
//     //         Ray ray = new Ray(cameraPoint.position,cameraPoint.forward );
//     //         RaycastHit hit;
//     //         Physics.Raycast(ray,  out hit, DashDistance);
//     //         if(hit.collider != null){
               
//     //             controller.enabled = false;
               
//     //             //make sure player doesnt teleport into object
//     //             float x = hit.point.x - controller.radius + controller.skinWidth;
//     //             float y = hit.point.y - controller.radius + controller.skinWidth;
//     //             float z = hit.point.z - controller.radius + controller.skinWidth;
                
//     //             playerTransform.position = new Vector3(x,y,z); 
//     //             controller.enabled = true;

//     //         }
//     //         else{
//     //             controller.enabled = false;
//     //              playerTransform.position += playerTransform.forward * DashDistance;
//     //              controller.enabled = true;
//     //         }

//     // }


//     IEnumerator Dash(){
       
      
//         Vector3 velocityCurrent = velocity;
//         velocity = Vector3.zero;
//         isDashing = true;
//         animationTimePosition = 0;
//         float startTime = Time.time;
//         nextPos = playerTransform.position;
      

//         Vector3 dashLocation = playerTransform.position + playerTransform.forward * (DashDistance - controller.radius - controller.skinWidth);
//         Ray ray = new Ray(playerTransform.position,playerTransform.forward );
//         RaycastHit hit;
//         Physics.Raycast(ray,  out hit, DashDistance);
//         if(hit.collider != null){
//          //   Debug.Log(hit.point + " " + dashLocation);
//             dashLocation = new Vector3(hit.point.x ,hit.point.y,hit.point.z - controller.radius - controller.skinWidth);
//            // Debug.Log("Starting Dash");
//         }
       
//         while(Vector3.Distance(playerTransform.position, dashLocation) > 0.1f){
          
            
//             float amount = dashCurve.Evaluate(animationTimePosition);
//             nextPos = Vector3.Lerp(playerTransform.position,dashLocation,(amount));
//             Debug.Log("Speed " + speed);
//             controller.Move((nextPos-playerTransform.position) * speed * Time.deltaTime);
//             float t = Time.deltaTime;
//             animationTimePosition += t;
           
//             yield return null;

//         }
    
//      //   Debug.Log("Ending Dash");
//         animationTimePosition = 0;
//        // Debug.Log("velocityCurrent : " + velocityCurrent);
//         velocity = velocityCurrent;
//         velocity.y = 0;
//         isDashing = false;

//     }

//     void disableControls(){
//         controls.Disable();
//     }
//     void enableControls(){
//         controls.Enable();
//     }

//     void OnDrawGizmos(){
//         if(Application.isPlaying){
//             Ray ray = new Ray(playerTransform.position,playerTransform.forward );
//             RaycastHit hit;
//             Physics.Raycast(ray,  out hit, DashDistance);
         
//             Gizmos.color = Color.yellow;
//             Vector3 pos;
//             if(hit.collider != null){
//                 pos = hit.point;                              
                
//             }else{
//                 pos = playerTransform.position + playerTransform.forward * DashDistance;
//             }
//             Gizmos.color = Color.red;
//             Gizmos.DrawSphere(nextPos,2);
//             Gizmos.color = Color.yellow;
//             Gizmos.DrawSphere(pos, 2);
//             Debug.DrawLine(playerTransform.position, pos);
//         }
//     }
// }
