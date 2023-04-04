using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechSpringEvents : MonoBehaviour
{
    private float velocityX,velocityY,velocityZ;
    private float restX,restY,restZ;
    private Vector3 currentJumpScale,currentRunScale;
    [SerializeField] private Transform playerTransform;
    [Header("Jump spring parameters")]
    [SerializeField, Range(0,1)]private float jumpSpringTime;
    [SerializeField, Range(0,100)]private float jumpSpringFrequency;
    [SerializeField, Range(0,10)]private float jumpSpringDrag;
    [SerializeField, Tooltip("The value you want the spring to be at its maximum")] Vector3 jumpSpringScale;

    [Header("Run spring parameters")]
    [SerializeField, Range(0,1)]private float runSpringTime;
    [SerializeField, Range(0,100)]private float runSpringFrequency;
    [SerializeField, Range(0,10)]private float runSpringDrag;
    [SerializeField, Tooltip("The value you want the spring to be at its maximum")] Vector3 runSpringScale;
    // Start is called before the first frame update
    private SpringUtils.tDampedSpringMotionParams _jumpParams = new SpringUtils.tDampedSpringMotionParams();
    private SpringUtils.tDampedSpringMotionParams _runParams = new SpringUtils.tDampedSpringMotionParams();
    void Start()
    {
        SpringUtils.CalcDampedSpringMotionParams(ref _jumpParams,0.2f,jumpSpringFrequency,jumpSpringDrag);
        SpringUtils.CalcDampedSpringMotionParams(ref _runParams,0.009f,runSpringFrequency,runSpringDrag);
        
        restX = playerTransform.localScale.x;
        restY = playerTransform.localScale.y;
        restZ = playerTransform.localScale.z;

        currentJumpScale = jumpSpringScale;
        Debug.Log("Jump Scale " + jumpSpringScale);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.parent == null)
        {
            if (Vector3.Distance(currentJumpScale, new Vector3(restX, restY, restZ)) > 0)
            {
                SpringUtils.UpdateDampedSpringMotion(ref currentJumpScale.x, ref velocityX, restX, _jumpParams);
                SpringUtils.UpdateDampedSpringMotion(ref currentJumpScale.y, ref velocityY, restY, _jumpParams);
                SpringUtils.UpdateDampedSpringMotion(ref currentJumpScale.z, ref velocityZ, restZ, _jumpParams);

                playerTransform.localScale = currentJumpScale;
            }

            if (Vector3.Distance(currentRunScale, new Vector3(restX, restY, restZ)) > 0)
            {
                SpringUtils.UpdateDampedSpringMotion(ref currentRunScale.x, ref velocityX, restX, _runParams);
                SpringUtils.UpdateDampedSpringMotion(ref currentRunScale.y, ref velocityY, restY, _runParams);
                SpringUtils.UpdateDampedSpringMotion(ref currentRunScale.z, ref velocityZ, restZ, _runParams);

                playerTransform.localScale = currentJumpScale;
            }
        }
    }


    public void OnSpringJump()
    {
        currentJumpScale = jumpSpringScale;
        
    }

    public void OnSpringRun()
    {
        currentRunScale = runSpringScale;
    }
}
