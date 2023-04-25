using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LedgeGrabState : IState{
    PlayerSM playerSM;
    Transform _playerTransform;
    Transform _wallCheck;
    private Transform _ledgeCheck;
    Rigidbody _rigidbody;
    private LayerMask groundLayer;
    PlayerControls controls;
    private LayerMask _groundLayer;
    private float gravity;
    float currentGravity;
    float initialJumpVelocity;
    float timeToPeak;
    private float _targetAngle;
    private float _angle;
    private int currentJumps = 1;
    private Vector2 _move;
    private bool _isGrounded;
    readonly float maxJumpHeight = 4.5f;
    readonly float maxJumpTime = .5f;
    readonly float wallCheckDistance = 0.3f;
    readonly float ledgeCheckDistance = 0.4f;
    readonly float dragForce = 8.5f;
    private bool _jumpPressed;
    private bool _jumping;
    private bool _onLedge;
    private bool _isLeft;
    private bool _isRight;
    private PlayerEventPublisher publisher;
    private Vector3 _force;
    private Vector3 ledgePoint;
    private Camera _camera;
    private float Speed = 200;
    private float _turnSmoothVelocity = 30;
    RaycastHit ledgeHit, wallHit;
    
    public LedgeGrabState(PlayerSM _playerSm, Rigidbody _rigidbody, Transform playerTransform, Transform _wallCheck,Transform _ledgeCheck, PlayerControls controls)
    {
        this.playerSM = _playerSm;
        this._rigidbody = _rigidbody;
        this._playerTransform = playerTransform;
        this._wallCheck = _wallCheck;
        this._ledgeCheck = _ledgeCheck;
        this.controls = controls;
        _camera = Camera.main;
    }
    public void Enter(){
        Debug.Log("Ledge grab state");
      //  publisher.updateStateChange("Ledge Grab");
       //controls = new PlayerControls();
        controls.LedgeGrab.Jump.performed += OnJump;
        controls.LedgeGrab.Jump.canceled += OnJump;
        groundLayer = LayerMask.GetMask("Level Geometry");
        timeToPeak = maxJumpTime/2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToPeak,2);
        initialJumpVelocity = 2 * maxJumpHeight/timeToPeak;
        currentGravity = gravity;
        _rigidbody.velocity = new Vector3();
        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

    }
    public void FixedTick()
    {
        _rigidbody.AddForce(_force,ForceMode.Force);
        Vector3 currentVelocity =  _rigidbody.velocity * ( 1 - Time.fixedDeltaTime * dragForce);
        if (!_onLedge) _rigidbody.velocity = new Vector3(currentVelocity.x, _rigidbody.velocity.y, currentVelocity.z);
        else _rigidbody.velocity = currentVelocity;
    }
    public void Tick()
    {
        Debug.Log(Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down * 17.5f, out ledgeHit,
            groundLayer));
        // Physics.SphereCast(_ledgeCheck.position, 1.5f,  Vector3.down, out ledgeHit, 17.5f ,groundLayer);
        ledgePoint = ledgeHit.point;
        Debug.Log(ledgePoint);
        Debug.Log(Physics.Linecast(_wallCheck.position, _wallCheck.position + _playerTransform.forward * 40.5f,
            out wallHit, groundLayer));
        _isLeft = (Physics.Linecast(_wallCheck.position +  Vector3.left  * 3.5f, _wallCheck.position + _playerTransform.forward * 10.5f,
            out wallHit, groundLayer));
        _isRight = (Physics.Linecast(_wallCheck.position + Vector3.right * 3.5f, _wallCheck.position + _playerTransform.forward * 10.5f,
            out wallHit, groundLayer));
       
        Debug.DrawRay(_ledgeCheck.position + Vector3.right * 2, Vector3.down * 2, Color.black, 3);
        Debug.Log(Physics.Linecast(_ledgeCheck.position, _ledgeCheck.position + Vector3.down * 17.5f, out ledgeHit,
            _groundLayer));
        Debug.DrawRay(_ledgeCheck.position, Vector3.down * 2, Color.black, 3);
        Debug.DrawRay(_ledgeCheck.position + Vector3.left * 2, Vector3.down * 2, Color.black, 3);
        
        
        HandleMoveInput();
        HandleRotation(); 
        
        


    }
    
    void HandleRotation(){
        if(_move.magnitude > 0.1 && (_isLeft && _isRight)){
            
            _targetAngle = Mathf.Atan2(_move.x, _move.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _angle = Mathf.SmoothDampAngle(_playerTransform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, 0.05f);
            Vector3 relativeForce = (Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward).normalized;

            _force.x = relativeForce.x * Speed * _move.magnitude;
            _force.z = relativeForce.z * Speed * _move.magnitude;
            Vector3 newForce = Vector3.ProjectOnPlane(_force, -1 * wallHit.normal);
            
            _force.x = newForce.x;
            _force.z = newForce.z;
            
            Debug.Log("new force " + newForce );
        }
        else {
            _force.x = 0;
            _force.z = 0;
        }
    }
    void ApplyGravity(){

        bool isFalling = ((_rigidbody.velocity.y <= 0.0f) || !_jumpPressed);
        float fallMultiplier = 15.0f;
        float previousYVelocity = _force.y;
        float nextYvelocity = 0;

        // Debug.Log("Falling + " + isFalling);

        if(isFalling ){
            float newYVelocity = _force.y + currentGravity * fallMultiplier;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
            
            //Debug.Log("Jump Test: " + ( jumpPressed) + " force " + Mathf.Max(nextYvelocity,-20f));
        }else{
            float newYVelocity = _force.y + currentGravity;
            nextYvelocity = (previousYVelocity + newYVelocity) * 0.5f;
        }

        if(!_onLedge)_force.y += Mathf.Max(nextYvelocity,-500) * Time.fixedDeltaTime; 
    }
    void HandleMoveInput()
    {
        _move = controls.GroundMove.Move.ReadValue<Vector2>();
        if (_move.y < 0)
        {
            resetConstraints();
            playerSM.ChangeState(playerSM._GroundMoveState);
        }

        if (_move.y > 0)
        {
            resetConstraints();
            _playerTransform.position = ledgePoint;
            playerSM.ChangeState(playerSM._GroundMoveState);
        }
        //else _force = new Vector3();

    }
    void resetConstraints()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    void OnJump(InputAction.CallbackContext context){
        Debug.Log("hi " + context.performed);
        _jumpPressed = context.performed;
        if(_jumpPressed && currentJumps > 0){
            publisher.updateJumpedStatus(-1);
        }
    }
    void HandleJump(){
        // Debug.Log("Jumping " + currentJumps );
        if(_jumpPressed && !_jumping){
            _rigidbody.AddForce(Vector3.up * initialJumpVelocity, ForceMode.Impulse);
            _jumping = true;
            currentJumps --;
            _jumpPressed = false;
            playerSM.ChangeState(playerSM._GroundMoveState);
            return;
        }

        if(_jumpPressed && currentJumps > 0 && _jumping){
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            _force.y = 0;
            _rigidbody.AddForce(Vector3.up * (initialJumpVelocity * 1.25f), ForceMode.Impulse);
            currentJumps --;
            _jumpPressed = false;
            return;
        }

        if(!_jumpPressed && _jumping){
            _jumping = false;
        }
    }
    public void Exit()
    {
        _force = new Vector3();
        controls.LedgeGrab.Jump.performed -= OnJump;
        controls.LedgeGrab.Jump.canceled  -= OnJump;
    }
}