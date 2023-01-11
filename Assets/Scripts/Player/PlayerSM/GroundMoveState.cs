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
    bool jumping = false;
    Vector3 force;
    Vector2 move;
    Vector3 prevPos;
    float turnSmoothVelocity = 6000;
    float speed = 300;
    float angle;
    float targetAngle;
    float currentSpeed;
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
        
        collider.center = new Vector3(0,6,0);
        collider.size = new Vector3(5f,12.14f,5f);
       
        groundLayer = LayerMask.GetMask("Level Geometry");
        waterLayer  = LayerMask.GetMask("Water");
        rigidbody = playerSM.GetComponent<Rigidbody>();
        force = new Vector3();
        Debug.Log("Ground move state");
        camera = Camera.main;
        publisher = new PlayerEventPublisher();
        publisher.updateGroundedStatus();
    }

    public void Tick(){
      


        CheckStatus();


        move = controls.GroundMove.Move.ReadValue<Vector2>();
        if(isGrounded && controls.GroundMove.Jump.ReadValue<float>() == 1)jumping = true;
        Debug.Log("Touching water "  + move.magnitude);
        Debug.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 5, Color.green, 0.1f, true);
        currentSpeed = Vector3.Distance(new Vector3(prevPos.x,0,prevPos.z), new Vector3(playerTransform.position.x, 0, playerTransform.position.z))/Time.deltaTime;
        publisher.updateSpeedStatus(move.magnitude);
        if(move.magnitude > 0.1){
            
            targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.01f);
            Vector3 relativeForce = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        
            force.x = relativeForce.x * speed;
            force.z = relativeForce.z * speed;
        }
       Debug.Log("force " + force + " move " + move + " grounded " + controls.GroundMove.Jump.ReadValue<float>());
    }

    public void FixedTick(){
        prevPos = playerTransform.position;
      
        if(!isGrounded && force.y >= -350)force.y += -9.8f;
        rigidbody.MoveRotation(Quaternion.Euler(0, angle  * Time.timeScale, 0));
        if(jumping == true)rigidbody.AddForce(Vector3.up * 15,ForceMode.Impulse);   
        rigidbody.AddForce(force);
        //basic decelleration should be swapped with a animation curve probably
        force.x *= 0.5f;
        force.z *= 0.5f;
    }

    public void Exit(){
          rigidbody.velocity = new Vector3(0,rigidbody.velocity.y, 0);
    }


    // This method is to check wether the mech should be in its land state or water state

    public void CheckStatus(){
        RaycastHit groundHit, waterHit;


        bool hitGround = Physics.Raycast(groundCheck.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer);
        bool hitWater  = Physics.Raycast(groundCheck.position, playerTransform.TransformDirection(Vector3.down), out waterHit,  15, waterLayer);
       
       
          Debug.Log("No ground hit " + hitGround + " " + groundHit.distance + " " + force + " isGrounded " + isGrounded);
        // Debug.Log("Ground " + groundHit.distance + " Water " + waterHit.distance + " distance between " + Vector3.Distance(groundHit.point, waterHit.point));

        if((hitGround) && groundHit.distance < waterHit.distance){
            if(Physics.CheckSphere(groundCheck.position, 1.5f, groundLayer)){
                isGrounded = true;
                force.y = 0;
                Debug.Log("Raycast hit ground and water but distance to ground was lesser");
            }
            else if(groundHit.distance > 1){
                isGrounded = false;
            }
            // else if(groundHit.distance > waterHit.distance && waterHit.distance <= 2){
            //     Debug.Log("Raycast hit ground and water but distance to ground was higher");
            //     //publisher.updateSubmergedStatus();
            //     //playerSM.ChangeState(playerSM.moveState);
            // }
        }

        if(hitGround && !hitWater){
            if(Physics.CheckSphere(groundCheck.position, 1.5f, groundLayer)){
                isGrounded = true;
                force.y = 0;
               
            }
            else 
                isGrounded = false;
        }

        if(hitGround == false && hitWater && waterHit.distance <= 0.8f){
           
            publisher.updateSubmergedStatus();
            playerSM.ChangeState(playerSM.moveState);
        }else {
            isGrounded = false;
        }

        // if(groundHit.distance < waterHit.distance && groundHit.distance <= 1){
        //     isGrounded = true; 
        //     force.y = 0;
        // }
        // if(groundHit.distance < waterHit.distance && groundHit.distance > 1){
        //     isGrounded = false;
        // }
        // if (groundHit.distance > waterHit.distance && waterHit.distance >=2 ){
        //     publisher.updateSubmergedStatus();
        //     playerSM.ChangeState(playerSM.moveState);
        // }

     
    }
}


   // if(( Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer) && 
        //     !Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out waterHit,  15, waterLayer)) && groundHit.distance <= 0.1){
        //     isGrounded = true; 
        //     force.y = 0;
        // }
        // else isGrounded = false;
        
        // if(Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out waterHit, 5, waterLayer) &&
        //   !Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.down), out groundHit, 15, groundLayer)){
          
          
        //     force = new Vector3();
        //     Debug.Log("Touching water "  + move.magnitude);
        //     aboveWater = true;
        //     publisher.updateSubmergedStatus();
        //     playerSM.ChangeState(playerSM.moveState);
        // }