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
    readonly Transform slopeCheck;
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
    private bool _onSlope;
    private bool _aiming;
    private bool lockOnPressed;
    float _turnSmoothVelocity = 6000;
    private const float Speed = 375;
    private bool attackPressed = false;
    private int currentAttack = 0;
    private int maxAttacks = 4;
    private float attackInputWindow = .75f;
    private float attackCounter = 0;

    private Vector3 orientation;
   
    float angle;
    float _targetAngle;
    float currentSpeed;
    private float slopeAngle;
    float currentYSpeed;
    private float slopeYForce;
    float gravity = -9.8f;
    readonly float groundedGravity = -75.25f;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    private float slopeLimit = 35;
    readonly float maxJumpHeight = 2.5f;
    readonly float maxJumpTime = .2f;
    readonly float wallCheckDistance = 0.3f;
    readonly float ledgeCheckDistance = 0.4f;
    readonly float dragForce = 8.5f;
    readonly int maxJumps = 2;
    int currentJumps;
    private bool swordEquipped = false;
    PlayerEventPublisher publisher;
    readonly CapsuleCollider collider;
    private Vector3 rotationAxis;
    private Vector3 slopeNormal;


    public GroundMoveState(PlayerSM playerSM, Rigidbody rigidboy, PlayerControls controls, Transform playerTransform, Transform groundCheck, Transform wallCheck, Transform ledgeCheck, Transform slopeCheck, CapsuleCollider collider){
        this._playerSm = playerSM;
        this._rigidbody = rigidboy;
        this._playerTransform = playerTransform;
        this._groundCheck = groundCheck;
        this._wallCheck = wallCheck;
        this._ledgeCheck = ledgeCheck;
        this._controls = controls;
        this.slopeCheck = slopeCheck; 
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
        _controls.GroundMove.Move.performed += OnMove;
        _controls.GroundMove.Move.canceled += OnMove;
        _controls.GroundMove.Dash.performed += OnDash;
        _controls.GroundMove.Dash.canceled += OnDash;
        _controls.GroundMove.Lockon.performed += HandleLockOn;
        _controls.GroundMove.Lockon.canceled += HandleLockOn;
        _controls.GroundMove.Sword.performed += HandleSword;
        _controls.GroundMove.Attack.performed += OnAttack;
        _controls.GroundMove.Aim.performed += OnAim;
        _controls.GroundMove.Aim.canceled += OnAim;

        _onSlope = false;
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

        if (_jumpPressed && (_isGrounded || _onSlope))
        {
            _force.y = 0;
        }
        else
        {
            _force.y += slopeYForce;
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


        Debug.Log("On slope " + _onSlope);
       
         //we only wanna check for ledges when we're falling TODO:: fix the ledge grab logic, its super finicky
        //  CheckForLedges();
        //tbh we dont need these as rigidbody contains a speed variable however, it doesnt keep track of horizontal and vertical speeds so i keep them just in case
        currentSpeed  = Vector3.Distance(new Vector3(_prevPos.x,0,_prevPos.z), new Vector3(_playerTransform.position.x, 0, _playerTransform.position.z))/Time.deltaTime;
        currentYSpeed = Vector3.Distance(new Vector3(0,_prevPos.y,0), new Vector3(0, _playerTransform.position.y, 0))/Time.deltaTime;
        SlopeCheck(); 
        HandleRotation();
        HandleAttack();
       
        
        //update the animator on our various attributes 
       // publisher.updateGroundedStatus(_isGrounded);
        publisher.updateAimStatus(_aiming);
        publisher.updateSpeedStatus(_move.magnitude);
        publisher.updateYSpeedStatus(-currentYSpeed);
        publisher.updateForce(_force);
        publisher.updateVelocity(_rigidbody.velocity);
        publisher.updateSubmerged(Physics.CheckSphere(_groundCheck.position, 0.25f , _waterLayer));

    }
    
    
    
    //On Input Methods
    void OnJump(InputAction.CallbackContext context){
        Debug.Log("jump performed " + context.performed);
        if(currentJumps > 0)
        {
            // currentJumps--;
            _jumpPressed = context.performed;
            _playerTransform.parent = null;
          
            if (_jumpPressed && currentJumps > 0)
            {
                _jumping = true;
                _force.y = 0;
                if(_isGrounded)
                    publisher.updateJumpedStatus(1);
                else
                    publisher.updateJumpedStatus(2);
                
                
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
    void OnAim(InputAction.CallbackContext context)
    {
        _aiming = context.ReadValueAsButton();
    }
    void OnAttack(InputAction.CallbackContext context)
    {
        attackPressed = context.performed;
        if (attackPressed)
        {
            attackCounter = attackInputWindow;
            
        }
    }
    
    //Status Checks for slopes, ledges, walls, ground etc...
    void CheckForLedges(){
        RaycastHit wallHit;
        RaycastHit ledgeHit;

        if (Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down, out ledgeHit, _groundLayer))
        {
            if (ledgeHit.normal == Vector3.up){
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

   
    void SlopeCheck(){
        RaycastHit downHit, nextDownHit;
        Vector3 newForce = new Vector3();
        Vector3 nextForce = new Vector3();
        //Make sure we are CURRENTLY standing on a slope by sending a raycast directly downward
        //Make sure we check for a slope in a slight distance in front of the player we do this so we know when to stop applying a force in the y component 
        //This prevents the character from flying off the top of slopes
        if(Physics.Raycast(_groundCheck.position + Vector3.up,Vector3.down, out downHit, 5f, _groundLayer))
        {
            Physics.Raycast(slopeCheck.position + Vector3.up,Vector3.down, out nextDownHit, 106, _groundLayer);
            Vector3 localHitNormal = _playerTransform.InverseTransformDirection(downHit.normal);
    
            slopeAngle = Vector3.Angle(localHitNormal, _groundCheck.up);
            if (downHit.normal != Vector3.zero)
                _onSlope = Vector3.Distance(downHit.normal, Vector3.up) > 0.2f;
    
          
    
            
            Debug.Log("Down Hit normal " + downHit.normal + " next hit normal " + nextDownHit.normal);
            if ((slopeAngle > 0.6f && slopeAngle < slopeLimit))
            {
                slopeNormal = downHit.normal * 1;
                newForce = Vector3.ProjectOnPlane(_force.normalized, downHit.normal);
                nextForce = Vector3.ProjectOnPlane(_force.normalized, nextDownHit.normal);
                Debug.Log("helloo " + newForce + " " + nextForce);
                if (_move.magnitude > 0.01f)
                {
                   
                    newForce = newForce.normalized * Speed;
                    nextForce = nextForce.normalized * Speed;
                    slopeYForce = newForce.y;
                    if (Mathf.Sign(newForce.y) == 1f) slopeYForce *= 0.5f;
                    else slopeYForce = -Speed * 0.5f;
                    _force = new Vector3(newForce.x, groundedGravity, newForce.z);
    
                }
               
                Debug.DrawRay(_groundCheck.position, newForce.normalized * 5, Color.white, 0.2f);
            }
            else if (slopeAngle > slopeLimit)
            {
    
                Vector3 downForce = Vector3.ProjectOnPlane(Vector3.down * Speed, downHit.normal);
                _force = downForce;
                Debug.Log("new Force " + newForce + " next force " + nextForce);
                //_isGrounded = false;
    
            }
            if (_isGrounded && Vector3.Distance(nextDownHit.normal, downHit.normal) > 0.2f && _onSlope)
            {
                if(Mathf.Sign(nextForce.y) == -1) Debug.Log("Going down a slope " + (nextDownHit.point.y  - _playerTransform.position.y));
                slopeYForce = Mathf.Lerp(slopeYForce,nextForce.y,Time.deltaTime * 5);
                // Debug.Log("helloo " + slopeYForce + " " + newForce.y);
                _rigidbody.velocity = Vector3.Lerp(new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z),_rigidbody.velocity,Time.deltaTime * 5);
            }
             if (_isGrounded && Vector3.Distance(downHit.normal, Vector3.up) < 0.2f)
            {
                if(Vector3.Distance(_force, nextDownHit.normal) > 0.2f)
                {
                    slopeYForce = nextForce.y;
                    Debug.Log("low force " + slopeYForce);
    
                }
            }
        }
        else
        {
            slopeNormal = Vector3.up;
            slopeYForce = 0;
           
            
        }
    }
    void CheckStatus()
    {
        RaycastHit groundHit, waterHit, platformHit;
        _isGrounded = Physics.Linecast(_groundCheck.position, _groundCheck.position + Vector3.down * 1, out groundHit, _groundLayer) || Physics.Linecast(_groundCheck.position, _groundCheck.position + Vector3.down * 1, out platformHit, _groundLayer);
        publisher.updateGroundedStatus(_isGrounded);
        if (Physics.Linecast(_groundCheck.position + Vector3.up, _groundCheck.position + Vector3.down, out platformHit, _MovingPlatform))
        {
            Rigidbody rb = platformHit.collider.gameObject.GetComponent<Rigidbody>();
            // _rigidbody.isKinematic = true;
            // _rigidbody.velocity = _rigidbody.velocity + rb.velocity;

        }
        else
        {
            _rigidbody.drag = 0;
        }


        if(_isGrounded)
        {
            if (_force.y > 0) _force.y = 0;
            else _force.y = groundedGravity;
            currentJumps = maxJumps;
            _jumping = false;
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
      
        _isGrounded = Physics.Linecast(_groundCheck.position, _groundCheck.position + Vector3.down * 1, out hit, _groundLayer);
        Debug.Log("Grounded " +  Physics.Linecast(_groundCheck.position, _groundCheck.position + Vector3.down, out hit, _groundLayer));
        if (_isGrounded)
        {
            _force.y = 0;
            currentJumps = maxJumps;
        }
        
    }

    //Handle Input methods
    void HandleJump(){ 
       //When on a slope a force is applied on the Y component either positive or negative depending on whether going up or down a slope
      
        Debug.Log("Force " + _force);
        if(_jumpPressed && (_isGrounded) && !_jumping)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _force.y = 0;
            _rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
            _jumping = true;
             currentJumps --;
            _jumpPressed = false;
            _playerSm.jumpDash = false;
            
            return;
        }
        //At this point when the button is pressed for the double jump the player is probably falling so theyre velocity is negative, so we reset it again in order to make jumps consistent 
        else if(_jumpPressed && currentJumps > 0 && _jumping && !_isGrounded){
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _force.y = 0;
            _rigidbody.AddForce(Vector3.up * (initialJumpVelocity * 1.25f), ForceMode.Impulse);
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
            
            
            newYVelocity = _force.y + currentGravity;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
        }

        if(!_isGrounded)_force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime; else _force.y += groundedGravity * Time.fixedDeltaTime;
    }
    void ApplyRotation(){
       
    
        //  Quaternion slopeRot = Quaternion.Slerp(_playerTransform.rotation,Quaternion.FromToRotation(Vector3.up, slopeNormal),Time.deltaTime * 5);
        // _rigidbody.MoveRotation( slopeRot * (Quaternion.Euler(0, angle , 0)));
        _rigidbody.MoveRotation((Quaternion.FromToRotation(Vector3.up, slopeNormal)) * (Quaternion.Euler(0, angle , 0)));
       
    }
    void HandleRotation(){
        if(_move.magnitude > 0.1){
            _targetAngle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.02f);
            Vector3 relativeForce = (Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward).normalized;

                publisher.UpdateDirection(_move);
                _force.x = relativeForce.x * Speed * _move.magnitude;
                _force.z = relativeForce.z * Speed * _move.magnitude;
            
        }
        else {
            _force.x = 0;
            _force.z = 0;
            publisher.UpdateDirection(new Vector2());
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
    void HandleAttack()
    {
        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        if (attackPressed && currentAttack >= 0 && currentAttack < maxAttacks)
        {
            currentAttack++;
            attackPressed = false;
            publisher.updateAttackStatus(currentAttack);
        }
        if (attackCounter <= 0)
        {
            currentAttack = 0;
            publisher.updateAttackStatus(-1);
        }
    }
    void HandleSword(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            swordEquipped = !swordEquipped;
            if (swordEquipped)
            {
                publisher.updateDrawSword();
            }

            if (!swordEquipped)
            {
                publisher.updateSheathSword();
                
            }
        }
    }
    public void Exit(){
        
        _controls.GroundMove.Jump.performed -= OnJump;
        _controls.GroundMove.Move.performed -= OnMove;
        _controls.GroundMove.Move.canceled  -= OnMove;
        _controls.GroundMove.Dash.performed -= OnDash;
        _controls.GroundMove.Dash.canceled  -= OnDash;
        _controls.GroundMove.Lockon.performed -= HandleLockOn;
        _controls.GroundMove.Lockon.canceled -= HandleLockOn;
        _controls.GroundMove.Sword.performed -= HandleSword;
        _controls.GroundMove.Attack.performed -= OnAttack;
        _controls.GroundMove.Aim.performed -= OnAim;
        _controls.GroundMove.Aim.canceled -= OnAim;
        _controls.Disable();
    }
}

