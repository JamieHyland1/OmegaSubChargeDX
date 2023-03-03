using UnityEngine;
using UnityEngine.InputSystem;
    
public class GroundMoveState : IState{
    readonly PlayerSM _playerSm;
    readonly PlayerControls _controls;
    LayerMask _groundLayer;
    LayerMask _waterLayer;
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
    float _turnSmoothVelocity = 6000;
    private const float Speed = 250;
    private const float airMoveSpeed = 200;
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
    readonly float maxJumpTime = .25f;
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

        collider.radius = 1.88f;
        collider.height = 9;
        collider.direction = 1;
        collider.center = new Vector3(0,4.65f,0);
       
        _groundLayer = LayerMask.GetMask("Level Geometry");
        _waterLayer  = LayerMask.GetMask("Water");
         // _rigidbody = _playerSm.GetComponent<Rigidbody>();
        _force = new Vector3();
        Debug.Log("Ground move state");
        _camera = Camera.main;
        
        publisher.updateOnLandStatus();

        _controls.GroundMove.Jump.performed += OnJump;
        _controls.GroundMove.Jump.canceled += OnJump;
        _controls.GroundMove.Move.performed += OnMove;
        _controls.GroundMove.Move.canceled += OnMove;
        _controls.GroundMove.Dash.performed += OnDash;
        _controls.GroundMove.Dash.canceled += OnDash;

        timeToPeak = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
        initialJumpVelocity = 2 * maxJumpHeight/timeToPeak;
        currentGravity = gravity;
        currentJumps = maxJumps;
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void FixedTick(){

        _prevPos = _playerTransform.position;  
       
        ApplyRotation();

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
         Debug.Log("Grounded " + _isGrounded);
        
        SlopeCheck();
        //we only wanna check for ledges when we're falling
        CheckForLedges();
        currentSpeed  = Vector3.Distance(new Vector3(_prevPos.x,0,_prevPos.z), new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z))/Time.deltaTime;
        currentYSpeed = Vector3.Distance(new Vector3(0,_prevPos.y,0), new Vector3(0, _playerTransform.position.y, 0))/Time.deltaTime;
        
        publisher.updateSpeedStatus(_move.magnitude);
        publisher.updateYSpeedStatus(-currentYSpeed);
        Debug.DrawLine(_prevPos, _playerTransform.position, Color.black,2f);
        GroundCheck();
        HandleRotation();
        
        
    }
    void OnJump(InputAction.CallbackContext context){
        _jumpPressed = context.performed;
        if(_jumpPressed && currentJumps > 0){
            publisher.updateJumpedStatus();
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

        if (Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down * 0.5f, out ledgeHit,_groundLayer))
        {
            //If we cant detect a wall below the edge it might not be a ledge
            Ray wallRay = new Ray(_wallCheck.position,_playerTransform.forward);
            if (!Physics.SphereCast(_wallCheck.position,2.5f,_playerTransform.forward,out wallHit, _groundLayer))
            {
                return;
            }
            if (wallHit.collider != null && wallHit.normal != Vector3.up)
            {
                 Quaternion ledgeRotation = Quaternion.LookRotation(-1 * wallHit.normal);
                 _playerTransform.rotation = ledgeRotation;
            }

           _rigidbody.velocity = new Vector3();
           _playerSm.ChangeState(_playerSm._LedgeGrabState);
        }
    }
    
    void HandleJump(){
        Debug.Log("Jumping " + _jumpPressed + " " + (currentJumps > 0) + " " + _jumping + " " + !_isGrounded);
        if(_jumpPressed && (_isGrounded) && !_jumping){
            _rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
            _jumping = true;
            currentJumps --;
            _jumpPressed = false;
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
            _angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.05f);
            Vector3 relativeForce = (Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward).normalized;

            if(_isGrounded)
            {
                _force.x = relativeForce.x * Speed * _move.magnitude;
                _force.z = relativeForce.z * Speed * _move.magnitude;
            }else if (!_isGrounded && _jumping)
            {
                _force.x = relativeForce.x * airMoveSpeed * _move.magnitude;
                _force.z = relativeForce.z * airMoveSpeed * _move.magnitude;
            }
        }
        else {
            _force.x = 0;
            _force.z = 0;
        }
    }
    // This method is to check wether the mech should be in its land state or water state
    void CheckStatus(){
        RaycastHit groundHit, waterHit;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, 0.4f , _groundLayer);
         publisher.updateGroundedStatus(_isGrounded);
        
        if(_isGrounded){
            _force.y = 0;
            currentJumps = maxJumps;
            Debug.Log("reset jumos");
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
        Physics.Raycast(_groundCheck.position + Vector3.up,_playerTransform.TransformDirection(Vector3.down), out downHit, 5.1f, _groundLayer);
        Vector3 localHitNormal = _playerTransform.InverseTransformDirection(downHit.normal);
        float slopeAngle  =  Vector3.Angle(localHitNormal,_groundCheck.up);
        if(slopeAngle != 0 && slopeAngle < 47.5f){
            Vector3 newForce = Vector3.ProjectOnPlane(_force,downHit.normal);
            newForce.y = Mathf.Min(newForce.y, 5);
            _force = newForce;
            Debug.DrawRay(_groundCheck.position, newForce, Color.white, 0.2f);
        }
    }
    public void Exit(){
        //   rigidbody.velocity = new Vector3(0,rigidbody.velocity.y, 0);
        _controls.GroundMove.Jump.performed -= OnJump;
        _controls.GroundMove.Jump.canceled  -= OnJump;
        _controls.GroundMove.Move.performed -= OnMove;
        _controls.GroundMove.Move.canceled  -= OnMove;
        _controls.GroundMove.Dash.performed -= OnDash;
        _controls.GroundMove.Dash.canceled  -= OnDash;
    }
}

