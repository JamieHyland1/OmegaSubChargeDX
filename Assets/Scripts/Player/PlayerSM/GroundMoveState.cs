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
    float speed = 350;
    float angle;
    float targetAngle;
    float currentSpeed;
    float currentYSpeed;
    float gravity = -9.8f;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    float maxJumpHeight = 5;
    float maxJumpTime = .5f;
    float dragForce = 6.5f;
    PlayerEventPublisher publisher;
    BoxCollider collider;


    public GroundMoveState(PlayerSM playerSM, Rigidbody rigidboy, PlayerControls controls, Transform playerTransform, Transform groundCheck, BoxCollider collider){
        this.playerSM = playerSM;
        this.rigidbody = rigidbody;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        this.controls = controls;
        this.collider = collider;

    }
    
    public void Enter(){
        
        collider.center = new Vector3(0,4.5f,0);
        collider.size = new Vector3(5f,9,5f);
       
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
        rigidbody.AddForce(force);
    
        Vector3 currentVelocity =  rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
        rigidbody.velocity = new Vector3(currentVelocity.x, rigidbody.velocity.y, currentVelocity.z);
        Debug.Log("velocity " + rigidbody.velocity + " force " + force);
        
        // force.x = 0f;
        // force.z = 0f;

         ApplyGravity();
         HandleJump();
     
    }




    public void Tick(){
      
        CheckStatus();
      
        move = controls.GroundMove.Move.ReadValue<Vector2>();
      
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

        if(!isGrounded)force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime;
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
    }

    // This method is to check wether the mech should be in its land state or water state

    public void CheckStatus(){
        RaycastHit groundHit, waterHit;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.25f , groundLayer);
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
        Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, groundLayer);
        Debug.Log("hit " + hit.point);
        groundPoint = hit.point;
    }

     public void Exit(){
        //   rigidbody.velocity = new Vector3(0,rigidbody.velocity.y, 0);
    }
}

