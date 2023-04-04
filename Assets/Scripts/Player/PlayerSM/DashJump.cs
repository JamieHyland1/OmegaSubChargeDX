using UnityEngine;
public class DashJump:IState
{
    private PlayerSM playerSM;
    private Rigidbody rigidbody;
    private Transform playerTransform;
    private Transform groundCheck;
    private PlayerControls controls;
    LayerMask groundLayer;
    LayerMask waterLayer;
    LayerMask movingPlatform;
    private Camera _camera;
    private Vector3 force;
    private Vector2 move;
    private bool isGrounded;
    
    readonly float maxJumpHeight = 4.25f;
    readonly float maxJumpTime = 0.7f;
    readonly float wallCheckDistance = 0.3f;
    readonly float ledgeCheckDistance = 0.4f;
    private  float _turnSmoothVelocity = 200;
    private float Speed = 900;
    private float moveSpeed = 1.5f;
    readonly float dragForce = 8.5f;
    private float _targetAngle = 0;
    private float angle;
    float gravity = -9.8f;
    readonly float groundedGravity = -5.25f;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    PlayerEventPublisher publisher;
    
    //TODO 
    //test if this code works lol
    //add logic to go back to ground move state after hitting ground again
    //tweak variables to get nice jump
    public DashJump(PlayerSM _playerSM, Rigidbody rigidbody, Transform playerTransform, Transform groundCheck, Transform waterSurfaceCheck, PlayerControls controls){
        this.playerSM = _playerSM;
        this.rigidbody = rigidbody;
        this.playerTransform = playerTransform;
        this.groundCheck = groundCheck;
        this.controls = controls;

    }
    public void Enter()
    {
        force = new Vector3();
        controls.Enable();
        _camera = Camera.main;
        timeToPeak = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
        initialJumpVelocity = (2 * maxJumpHeight)/timeToPeak;
        currentGravity = gravity;
        publisher = new PlayerEventPublisher();
        publisher.updateStateChange("Dash jump");
        publisher.playDashEvent();
        rigidbody.velocity = playerSM.storedVelocty;
        Vector3 upVel = Vector3.up * initialJumpVelocity;
        Vector3 forwardVel = new Vector3(playerTransform.forward.x, 0, playerTransform.forward.z) * Speed;
        force = new Vector3(forwardVel.x, upVel.y, forwardVel.z);
        Debug.Log("Jump state force " + force);
         rigidbody.AddForce(upVel * 0.42f, ForceMode.Impulse);
        groundLayer = LayerMask.GetMask("Level Geometry");
        waterLayer  = LayerMask.GetMask("Water");
        movingPlatform = LayerMask.GetMask("MovingPlatforms");
    

    }
    public void Tick()
    {
        if (playerTransform.parent != null) playerTransform.parent = null;
        move = controls.GroundMove.Move.ReadValue<Vector2>();
        HandleRotation();
        publisher.updateForce(force);
        publisher.updateVelocity(rigidbody.velocity);
        publisher.updateSubmerged(Physics.CheckSphere(groundCheck.position, 0.25f , waterLayer));
        // if (force.magnitude > Speed)
        // {
        //     
        //     force = force.normalized * Speed;
        // }
    }
    public void FixedTick()
    {
        ApplyRotation();
        rigidbody.AddForce(force, ForceMode.Force);
        // rigidbody.AddForce(force, ForceMode.Acceleration);
        Vector3 currentVelocity =  rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
        if(!isGrounded)rigidbody.velocity = new Vector3(currentVelocity.x, rigidbody.velocity.y, currentVelocity.z); else rigidbody.velocity = currentVelocity;
      
        ApplyGravity();
        GroundCheck();
    }
    public void ApplyGravity()
    {
        
        float fallMultiplier = 2.5f;
        float previousYVelocity = force.y;
        float nextYvelocity = 0;
        float newYVelocity;
        
        newYVelocity = force.y + currentGravity * fallMultiplier;
        nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
        
        if(!isGrounded)force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime; else force.y += groundedGravity * Time.fixedDeltaTime;
    }
    void ApplyRotation(){
        rigidbody.MoveRotation(Quaternion.Euler(0, angle  * Time.timeScale, 0));
    }
    void HandleRotation()
    {
        if(move.magnitude > 0)
        {
            Vector3 relativeForce = new Vector3();
            _targetAngle = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 10.75f);
            relativeForce = (Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward).normalized;


            force.x += relativeForce.x * moveSpeed;
            force.z += relativeForce.z * moveSpeed;

            if (Mathf.Abs(force.x) > Speed) force.x = Speed * Mathf.Sign(force.x);
            if (Mathf.Abs(force.z) > Speed) force.z = Speed * Mathf.Sign(force.z);
        }
         
    }
    void GroundCheck(){
        RaycastHit groundHit, waterHit, platformHit;
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundLayer) ||
                      Physics.CheckSphere(groundCheck.position, 0.4f, movingPlatform);
        if (Physics.Linecast(groundCheck.position + Vector3.up, groundCheck.position + Vector3.down, out platformHit, movingPlatform))
        {
           
            Rigidbody rb = platformHit.collider.gameObject.GetComponent<Rigidbody>();
          
        }
        else
        {
            rigidbody.drag = 0;
        }


        if(isGrounded){
            force.y = 0;
            playerSM.ChangeState(playerSM._GroundMoveState);
        }
        if(!isGrounded){
            if( (Physics.OverlapSphere(playerTransform.position, 0.5f, waterLayer).Length > 0)){
                publisher.updateYSpeedStatus(0);
                publisher.updateSubmergedStatus();
                playerSM.ChangeState(playerSM._WaterMoveState);
            }
        }
    }
    public void Exit()
    {
        
    }
}
