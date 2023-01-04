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
    Vector3 force;
    Vector2 move;
    float turnSmoothVelocity = 6000;
    float speed = 300;
    
    public GroundMoveState(PlayerSM playerSM, Rigidbody rigidboy, PlayerControls controls, Transform playerTransform, Transform groundCheck){
        this.playerSM = playerSM;
        this.rigidbody = rigidbody;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        this.controls = controls;
        

    }
    
    public void Enter(){
        groundLayer = LayerMask.GetMask("Level Geometry");
        waterLayer  = LayerMask.GetMask("Water");
        rigidbody = playerSM.GetComponent<Rigidbody>();
        force = new Vector3();
        Debug.Log("Ground move state");
        camera = Camera.main;
    }

    public void Tick(){
       if (Physics.CheckSphere(groundCheck.position, 0.1f,groundLayer)){
            isGrounded = true;
            force.y = 0;
        }else{
            isGrounded = false;

        }
        if(Physics.CheckSphere(groundCheck.position, 0.1f,waterLayer)){
            force = new Vector3();
            Debug.Log("Touching water");
            aboveWater = true;
            playerSM.ChangeState(playerSM.moveState);
        }
        move = controls.GroundMove.Move.ReadValue<Vector2>();
       
        // force = new Vector3(playerTransform.forward.x * move.x * speed, force.y, playerTransform.forward.z * move.y * speed);
    }

    public void FixedTick(){
      
        float targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
        if(!isGrounded)force.y += -9.8f;
        rigidbody.MoveRotation(Quaternion.Euler(0, angle  * Time.timeScale, 0));
        Vector3 relativeForce = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        Debug.Log("move: " + move + " force: " + force + " relative force: " + relativeForce + aboveWater);
        force.x = relativeForce.x * speed;
        force.z = relativeForce.z * speed;
        rigidbody.AddForce(force);
    }

    public void Exit(){

    }
}