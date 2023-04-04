using UnityEngine;
using UnityEngine.InputSystem;
    
public class GroundMoveState : IState{
    readonly PlayerSM _playerSm;
    readonly PlayerControls _controls;
    LayerMask _groundLayer;
    LayerMask _waterLayer;
    LayerMask _MovingPlatform;
    readonly Transform _playerTransform;
    readonly Transform _groundCheck;
    readonly Transform _wallCheck;
    readonly Transform _ledgeCheck;
    Rigidbody _rigidbody;
    Camera _camera;
   
    Vector3 _force;
    Vector2 _move;
    Vector3 _prevPos;
    Vector3 _prevVel;
    bool _isGrounded;
    bool _aboveWater = false;
    bool _jumping;
    bool _jumpPressed;
    bool _jumpHeld;
    bool _dashing;
    private bool lockOnPressed;
    float _turnSmoothVelocity = 6000;
    private const float Speed = 375;
   
    float _angle;
    float _targetAngle;
    float currentSpeed;
    float currentYSpeed;
    float gravity = -9.8f;
    readonly float groundedGravity = -5.25f;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    readonly float maxJumpHeight = 2.5f;
    readonly float maxJumpTime = .2f;
    readonly float wallCheckDistance = 0.3f;
    readonly float ledgeCheckDistance = 0.4f;
    readonly float dragForce = 8.5f;
    readonly int maxJumps = 2;
    int currentJumps;
    PlayerEventPublisher publisher;
    readonly CapsuleCollider collider;


    public GroundMoveState(PlayerSM playerSM, Rigidbody rigidboy, PlayerControls controls, Transform playerTransform, Transform groundCheck, Transform wallCheck, Transform ledgeCheck, CapsuleCollider collider){
        this._playerSm = playerSM;
        this._rigidbody = rigidboy;
        this._playerTransform = playerTransform;
        this._groundCheck = groundCheck;
        this._wallCheck = wallCheck;
        this._ledgeCheck = ledgeCheck;
        this._controls = controls;
        this.collider = collider;

    }
    public void Enter(){
        publisher = new PlayerEventPublisher();
        publisher.changeToMechCamera();
        publisher.updateStateChange("Mech");
        _controls.Enable();
        
        collider.radius = 1.88f;
        collider.height = 10;
        collider.direction = 1;
        collider.center = new Vector3(0,4.65f,0);
       
        _groundLayer = LayerMask.GetMask("Level Geometry");
        _waterLayer  = LayerMask.GetMask("Water");
        _MovingPlatform = LayerMask.GetMask("MovingPlatforms");
         // _rigidbody = _playerSm.GetComponent<Rigidbody>();
        _force = new Vector3();
        _camera = Camera.main;
        
        publisher.updateOnLandStatus();
        _rigidbody.drag = 0;

        _controls.GroundMove.Jump.performed += OnJump;
        _controls.GroundMove.Jump.canceled += OnJump;
        _controls.GroundMove.Move.performed += OnMove;
        _controls.GroundMove.Move.canceled += OnMove;
        _controls.GroundMove.Dash.performed += OnDash;
        _controls.GroundMove.Dash.canceled += OnDash;
        _controls.GroundMove.Lockon.performed += HandleLockOn;
        _controls.GroundMove.Lockon.canceled += HandleLockOn;
        
        timeToPeak = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
        initialJumpVelocity = 2 * maxJumpHeight/timeToPeak;
        currentGravity = gravity;
        currentJumps = maxJumps;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        if (_playerSm.storedVelocty.magnitude > 0.1f)
        {
            _rigidbody.velocity = _playerSm.storedVelocty;
            _playerSm.storedVelocty = new Vector3();
        }
    }
    public void FixedTick(){

        _prevPos = _playerTransform.position;

        if (!lockOnPressed)
        {
            ApplyRotation();
        }

        _rigidbody.AddForce(_force, ForceMode.Force);
    
        // The Drag component of unitys rigidbody is messing with our jump formula, so set  the rigidbodys drag to 0 in the inspector and add the drag to the x,z axis ourselves leaving the y component dragless

        Vector3 currentVelocity =  _rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
        if(!_isGrounded)_rigidbody.velocity = new Vector3(currentVelocity.x, _rigidbody.velocity.y, currentVelocity.z); else _rigidbody.velocity = currentVelocity;
 
         ApplyGravity();
         HandleJump();
     
    }
    public void Tick(){
      
        CheckStatus();
      
        _move = _controls.GroundMove.Move.ReadValue<Vector2>();
        
        SlopeCheck();
        //we only wanna check for ledges when we're falling
      //  CheckForLedges();
        currentSpeed  = Vector3.Distance(new Vector3(_prevPos.x,0,_prevPos.z), new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z))/Time.deltaTime;
        currentYSpeed = Vector3.Distance(new Vector3(0,_prevPos.y,0), new Vector3(0, _playerTransform.position.y, 0))/Time.deltaTime;
       
        
        
        publisher.updateSpeedStatus(_move.magnitude);
        publisher.updateYSpeedStatus(-currentYSpeed);
        Debug.DrawLine(_prevPos, _playerTransform.position, Color.black,2f);
        GroundCheck();
        HandleRotation();
      Debug.Log("Grounded " + _isGrounded);
        publisher.updateForce(_force);
        publisher.updateVelocity(_rigidbody.velocity);
        publisher.updateSubmerged(Physics.CheckSphere(_groundCheck.position, 0.25f , _waterLayer));

    }
    void OnJump(InputAction.CallbackContext context){
        Debug.Log(context.performed);
        if(currentJumps > 0)
        {
            // currentJumps--;
            _jumpPressed = context.performed;
            _playerTransform.parent = null;
            if (_jumpPressed && currentJumps > 0)
            {
                publisher.updateJumpedStatus();
                
            }
        }
    }
    void OnMove(InputAction.CallbackContext context){
        // Debug.Log("Move " + context);
    }
    void OnDash(InputAction.CallbackContext context){
        _dashing = context.ReadValueAsButton();
        // Debug.Log("Dashing " + _dashing);
        if(_dashing && _isGrounded){
            _rigidbody.velocity = new Vector3();
            _force = new Vector3();
            _playerSm.ChangeState(_playerSm._DshState);
        }
    }
    
    void CheckForLedges(){
        RaycastHit wallHit;
        RaycastHit ledgeHit;

        if (Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down, out ledgeHit, _groundLayer))
        {
            // Debug.Log("Ledge " + Vector3.Distance(ledgeHit.normal, Vector3.up) + ledgeHit.normal);
            if (ledgeHit.normal == Vector3.up){
                //If we cant detect a wall below the edge it might not be a ledge
                Ray wallRay = new Ray(_wallCheck.position, _playerTransform.forward);
            if (!Physics.SphereCast(_wallCheck.position, 2.5f, _playerTransform.forward, out wallHit, _groundLayer))
            {
                return;
            }

            if (wallHit.collider != null && wallHit.normal != Vector3.up &&
                Vector3.Distance(ledgeHit.normal, Vector3.up) < 0.2f)
            {
                Quaternion ledgeRotation = Quaternion.LookRotation(-1 * wallHit.normal);
                _playerTransform.rotation = ledgeRotation;
            }

            _rigidbody.velocity = new Vector3();
            _playerSm.ChangeState(_playerSm._LedgeGrabState);
        }
    }
    }
    
    void HandleJump(){
        if(_jumpPressed && (_isGrounded) && !_jumping){
            _rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
            _jumping = true;
             currentJumps --;
            _jumpPressed = false;
            _playerSm.jumpDash = false;
            return;
        }
        else if(_jumpPressed && currentJumps > 0 && _jumping && !_isGrounded){
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _force.y = 0;
            _rigidbody.AddForce(Vector3.up * (initialJumpVelocity * 1.0f), ForceMode.Impulse);
             currentJumps --;
            _jumpPressed = false;
            return;
        }
        else if((!_jumpPressed && _isGrounded && _jumping) || currentJumps <= 0){
        _jumping = false;
        }
    }
    void ApplyGravity(){

        bool isFalling = ((_rigidbody.velocity.y <= 0.0f) || !_jumpPressed);
        float fallMultiplier = 15.0f;
        float previousYVelocity = _force.y;
        float nextYvelocity = 0;
        float newYVelocity;
        if(isFalling){ 
            newYVelocity = _force.y + currentGravity * fallMultiplier;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            
            
            //Debug.Log("Jump Test: " + ( jumpPressed) + " force " + Mathf.Max(nextYvelocity,-20f));
            newYVelocity = _force.y + currentGravity;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
        }

        if(!_isGrounded)_force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime; else _force.y += groundedGravity * Time.fixedDeltaTime;
    }
    void ApplyRotation(){
         _rigidbody.MoveRotation(Quaternion.Euler(0, _angle  * Time.timeScale, 0));
    }
    void HandleRotation(){
        if(_move.magnitude > 0.1){
            _targetAngle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.02f);
            Vector3 relativeForce = (Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward).normalized;

          
                _force.x = relativeForce.x * Speed * _move.magnitude;
                _force.z = relativeForce.z * Speed * _move.magnitude;
            
        }
        else {
            _force.x = 0;
            _force.z = 0;
        }
    }
    // This method is to check wether the mech should be in its land state or water state
    void CheckStatus()
    {
        RaycastHit groundHit, waterHit, platformHit;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, 0.4f, _groundLayer) ||
                      Physics.CheckSphere(_groundCheck.position, 0.4f, _MovingPlatform);
        publisher.updateGroundedStatus(_isGrounded);
        if (Physics.Linecast(_groundCheck.position + Vector3.up, _groundCheck.position + Vector3.down, out platformHit, _MovingPlatform))
        {
            // Debug.Log("on moving platform");
            Rigidbody rb = platformHit.collider.gameObject.GetComponent<Rigidbody>();
            // _rigidbody.isKinematic = true;
            // _rigidbody.velocity = _rigidbody.velocity + rb.velocity;

        }
        else
        {
            _rigidbody.drag = 0;
        }


        if(_isGrounded){
            _force.y = 0;
            currentJumps = maxJumps;
        }
        if(!_isGrounded){
            if( Physics.CheckSphere(_groundCheck.position, 0.25f , _waterLayer)){
                publisher.updateYSpeedStatus(0);
                publisher.updateSubmergedStatus();
                _playerSm.ChangeState(_playerSm._WaterMoveState);
            }
        }
    }
    void GroundCheck(){
        RaycastHit hit;
        Physics.Raycast(_groundCheck.position, Vector3.down, out hit, 0.2f, _groundLayer);
    }
    void SlopeCheck(){
        RaycastHit downHit;
        Physics.Raycast(_groundCheck.position + Vector3.up,Vector3.down, out downHit, 5.1f, _groundLayer);
        Vector3 localHitNormal = _playerTransform.InverseTransformDirection(downHit.normal);
        float slopeAngle  =  Vector3.Angle(localHitNormal,_groundCheck.up);
        Debug.Log("Grounded Slope Angle " + slopeAngle);
        if (slopeAngle != 0 && slopeAngle < 47.5f)
        {
            Vector3 newForce = Vector3.ProjectOnPlane(_force, downHit.normal);
            newForce.y = Mathf.Min(newForce.y, 5);
            _force = newForce;
            Debug.DrawRay(_groundCheck.position, newForce, Color.white, 0.2f);
        }
        else if (slopeAngle > 47.5f) 
        {
            Vector3 downForce = Vector3.ProjectOnPlane(Vector3.down * Speed,downHit.normal);
            _force = downForce;
            Debug.Log("Grounded stee[ Slope " + slopeAngle);
            

        }
    }
    void HandleLockOn(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            lockOnPressed = !lockOnPressed;
            Debug.Log("Lock on event " + lockOnPressed);
        }
        publisher.targetEnemy(lockOnPressed);
       
    }
    
    public void Exit(){
        
        _controls.GroundMove.Jump.performed -= OnJump;
        _controls.GroundMove.Jump.canceled  -= OnJump;
        _controls.GroundMove.Move.performed -= OnMove;
        _controls.GroundMove.Move.canceled  -= OnMove;
        _controls.GroundMove.Dash.performed -= OnDash;
        _controls.GroundMove.Dash.canceled  -= OnDash;
        _controls.GroundMove.Lockon.performed -= HandleLockOn;
        _controls.GroundMove.Lockon.canceled -= HandleLockOn;
        _controls.Disable();
    }
}

