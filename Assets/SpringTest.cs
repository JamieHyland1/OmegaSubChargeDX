using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpringTest : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 springScale, currentScale;
    private float velocityX,velocityY,velocityZ;
    private float restX,restY,restZ;
    [SerializeField, Range(0,1)]private float t;
    [SerializeField, Range(0,100)]private float f;
    [SerializeField, Range(0,10)]private float d;
    private PlayerControls _controls;
    private bool pause = false;


    private SpringUtils.tDampedSpringMotionParams _params = new SpringUtils.tDampedSpringMotionParams();
    void Start()
    {
        springScale = this.transform.localScale;
        _controls = new PlayerControls();
        _controls.Enable();
        restX = transform.localScale.x;
        restY = transform.localScale.y;
        restZ = transform.localScale.z;
        springScale.x *= .5f;
        springScale.y *= 1.55f;
        springScale.z *= .5f;
        currentScale = springScale;
        velocityX = 0;
        velocityY = 0;
        velocityZ = 0;
        _controls.GroundMove.Jump.performed += OnJump;
        SpringUtils.CalcDampedSpringMotionParams(ref _params,0.009f,f,d);
    }

    // Update is called once per frame
    void Update()
    {
        // t += Time.deltaTime;
        if(!pause)
        {
            SpringUtils.UpdateDampedSpringMotion(ref currentScale.x, ref velocityX, restX, _params);
            SpringUtils.UpdateDampedSpringMotion(ref currentScale.y, ref velocityY, restY, _params);
            SpringUtils.UpdateDampedSpringMotion(ref currentScale.z, ref velocityZ, restZ, _params);
            transform.localScale = currentScale;
        }
    }

    public void OnHit()
    {
        currentScale = springScale;
    }

    public void Pause()
    {
        pause = true;
    }
    
    public void Play()
    {
        pause = false;
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentScale = springScale;
        }
    }
}
