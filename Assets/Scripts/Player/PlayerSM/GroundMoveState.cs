using UnityEngine;
using UnityEngine.InputSystem;
    
public class GroundMoveState : IState{
    PlayerSM playerSM;
    PlayerControls controls;
    LayerMask groundLayer;
    LayerMask waterLayer;
    Transform playerTransform;
    Transform groundCheck;
    GameObject boostEffectObj;
    AnimationCurve accelCurve;
    Rigidbody rigidbody;
    Camera camera;
    bool isGrounded;
    bool aboveWater = false;
    bool jumping;
    bool jumpPressed;
    Vector3 force;
    Vector2 move;
    Vector3 prevPos;
    Vector3 prevVel;
    Vector3 groundPoint;
    float turnSmoothVelocity = 6000;
    float speed = 400;
    float angle;
    float targetAngle;
    float currentSpeed;
    float currentYSpeed;
    float gravity = -9.8f;
    float groundedGravity = -5.25f;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    float maxJumpHeight = 5;
    float maxJumpTime = .5f;
    float dragForce = 8.5f;
    PlayerEventPublisher publisher;
    CapsuleCollider collider;


    public GroundMoveState(PlayerSM playerSM, Rigidbody rigidboy, PlayerControls controls, Transform playerTransform, Transform groundCheck, CapsuleCollider collider){
        this.playerSM = playerSM;
        this.rigidbody = rigidbody;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        this.controls = controls;
        this.collider = collider;

    }
    
    public void Enter(){
        
        //height 9
        // radius 1.888
        // direction z
        // collider.center = new Vector3(0,4.5f,0);
        // collider.size = new Vector3(5f,9,5f);

        collider.radius = 1.88f;
        collider.height = 9;
        collider.direction = 1;
        collider.center = new Vector3(0,4.65f,0);
       
        groundLayer = LayerMask.GetMask("Level Geometry");
        waterLayer  = LayerMask.GetMask("Water");
        rigidbody = playerSM.GetComponent<Rigidbody>();
        force = new Vector3();
        Debug.Log("Ground move state");
        camera = Camera.main;
        publisher = new PlayerEventPublisher();
        // /publisher.updateGroundedStatus(true);
        publisher.updateOnLandStatus();

        controls.GroundMove.Jump.performed += OnJump;
        controls.GroundMove.Jump.canceled += OnJump;
        controls.GroundMove.Move.performed += OnMove;
        controls.GroundMove.Move.canceled += OnMove;

        timeToPeak = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
        initialJumpVelocity = 2 * maxJumpHeight/timeToPeak;
        currentGravity = gravity;
    }


    public void FixedTick(){

        prevPos = playerTransform.position;  
        prevVel = rigidbody.velocity;
           // The Drag component of unitys rigidbody is messing with our jump formula, so set  the rigidbodys drag to 0 in the inspector and add the drag to the x,z axis ourselves leaving the y component dragless
       
       
        ApplyRotation();

       
        //apply force to rigidbody
        rigidbody.AddForce(force, ForceMode.Force);
    
        Vector3 currentVelocity =  rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
        if(!isGrounded)rigidbody.velocity = new Vector3(currentVelocity.x, rigidbody.velocity.y, currentVelocity.z); else rigidbody.velocity = currentVelocity;
        Debug.Log("velocity " + rigidbody.velocity + " force " + force);
        
        // force.x = 0f;
        // force.z = 0f;

         ApplyGravity();
         HandleJump();
     
    }




    public void Tick(){
      
        CheckStatus();
      
        move = controls.GroundMove.Move.ReadValue<Vector2>();
        
        SlopeCheck();

        currentSpeed  = Vector3.Distance(new Vector3(prevPos.x,0,prevPos.z), new Vector3(playerTransform.position.x, 0, playerTransform.position.z))/Time.deltaTime;
        currentYSpeed = Vector3.Distance(new Vector3(0,prevPos.y,0), new Vector3(0, playerTransform.position.y, 0))/Time.deltaTime;
        
        publisher.updateSpeedStatus(move.magnitude);
        publisher.updateYSpeedStatus(-currentYSpeed);
        Debug.DrawLine(prevPos, playerTransform.position, Color.black,2f);
        GroundCheck();
        HandleRotation();
        
        
    }

    void OnJump(InputAction.CallbackContext context){
      
     
        jumpPressed = context.ReadValueAsButton();
        if(jumpPressed)publisher.updateJumpedStatus();

    }

    void OnMove(InputAction.CallbackContext context){
        Debug.Log("Move " + context);
    }
    

    void HandleJump(){
        if(jumpPressed && isGrounded && !jumping){
            
            rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
            jumping = true;
        }
        else if(!jumpPressed && isGrounded && jumping){
            jumping = false;
        }   
    }

    void ApplyGravity(){

        bool isFalling = ((rigidbody.velocity.y <= 0.0f) || !jumpPressed);
        float fallMultiplier = 15.0f;
        float previousYVelocity = force.y;
        float nextYvelocity = 0;

        // Debug.Log("Falling + " + isFalling);

        if(isFalling){
            float newYVelocity = force.y + currentGravity * fallMultiplier;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            
            //Debug.Log("Jump Test: " + ( jumpPressed) + " force " + Mathf.Max(nextYvelocity,-20f));
        }else{
            float newYVelocity = force.y + currentGravity;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
        }

        if(!isGrounded)force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime; else force.y += groundedGravity * Time.fixedDeltaTime;
    }

    void ApplyRotation(){
         rigidbody.MoveRotation(Quaternion.Euler(0, angle  * Time.timeScale, 0));
    }

    void HandleRotation(){
        if(move.magnitude > 0.1){
            
            targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.05f);
            Vector3 relativeForce = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

            Debug.Log("Move " + move.magnitude);

            force.x = relativeForce.x * speed * move.magnitude;
            force.z = relativeForce.z * speed * move.magnitude;
        }
        else {
            force.x = 0;
            force.z = 0;
        }
        // Debug.DrawRay(groundCheck.position, playerTransform.forward * 5, Color.magenta,0.2f,  false);
    }

    // This method is to check wether the mech should be in its land state or water state

    public void CheckStatus(){
        RaycastHit groundHit, waterHit;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.55f , groundLayer);
         publisher.updateGroundedStatus(isGrounded);
        
        if(isGrounded){
            force.y = 0;
            
           
          
        }

        if(!isGrounded){
            if( Physics.CheckSphere(groundCheck.position, 0.25f , waterLayer)){
                publisher.updateYSpeedStatus(0);
                publisher.updateSubmergedStatus();
                playerSM.ChangeState(playerSM.moveState);
            }
        }
       
    }

    public void GroundCheck(){
        RaycastHit hit;
        Physics.Raycast(groundCheck.position, Vector3.down, out hit, 0.2f, groundLayer);
        Debug.Log("hit " + hit.distance);
        groundPoint = hit.point;
        if(hit.distance <= 8){
            
           // playerTransform.position = new Vector3(playerTransform.position.x, hit.point.y, playerTransform.position.z);
        
        }
    }


    public void SlopeCheck(){
        RaycastHit downHit;
       
        Physics.Raycast(groundCheck.position + Vector3.up,playerTransform.TransformDirection(Vector3.down), out downHit, 5.1f, groundLayer);
        // Physics.Raycast(groundCheck.position + Vector3.up,playerTransform.TransformDirection(Vector3.down), out downHit, 1.1f, groundLayer);
        Debug.DrawRay(groundCheck.position, downHit.normal , Color.red ,5f);

        
        Vector3 localHitNormal = playerTransform.InverseTransformDirection(downHit.normal);
        
        float slopeAngle  =  Vector3.Angle(localHitNormal,groundCheck.up);
        Debug.Log("Slope " + slopeAngle);
          if(slopeAngle != 0 && slopeAngle < 47.5f){
            // Quaternion slopeAngleRotation = Quaternion.FromToRotation(groundCheck.up,localHitNormal);
            // Debug.Log("rotation " + slopeAngleRotation);
            // Vector3 newForce = (slopeAngleRotation * force);
            // newForce.y *=  -1;
            // Debug.DrawRay(groundCheck.position, newForce , Color.red ,5f);
            // Debug.Log("New Force " + newForce );
            // force = newForce;

            Vector3 newForce = Vector3.ProjectOnPlane(force,downHit.normal);
          
            // newForce.y +=  downHit.normal.y;
            newForce.y = Mathf.Min(newForce.y, 5);
              Debug.Log("normal " + newForce.y );
            force = newForce;
           Debug.DrawRay(groundCheck.position, newForce, Color.white, 0.2f);
           // transform.rotation = slopeAngleRotation;
           // force.y *= -1;
          }
        
    }

     public void Exit(){
        //   rigidbody.velocity = new Vector3(0,rigidbody.velocity.y, 0);
    }
}

