using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeTest : MonoBehaviour
{
    [SerializeField] private Transform downRay;
    [SerializeField] private Transform nextRay;
    Vector3 force;
    LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {
        force = downRay.forward * 350;
        groundLayer = LayerMask.GetMask("Level Geometry");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(downRay.position, Vector3.down * 2.5f, Color.green,0.5f);
        Debug.DrawRay(nextRay.position, Vector3.down * 2.5f, Color.green,0.5f);
        CheckSlope(nextRay, Color.red);
       // CheckSlope(downRay, Color.blue);
    }

    void FixedUpdate(){
      // CheckSlope(downRay, Color.blue);
      // CheckSlope(downRay, Color.red);
        }

        void CheckSlope(Transform t, Color c){
             RaycastHit downHit;
       
        Physics.Raycast(t.position + Vector3.up,this.transform.TransformDirection(Vector3.down), out downHit, Mathf.Infinity, groundLayer);
        
        Vector3 localHitNormal = this.transform.InverseTransformDirection(downHit.normal);
        
        float slopeAngle  =  Vector3.Angle(localHitNormal,t.up);
        Debug.Log("Slope " + slopeAngle);
        // if(slopeAngle != 0){
            Quaternion slopeAngleRotation = Quaternion.FromToRotation(t.up,localHitNormal);
            Debug.Log("rotation " + slopeAngleRotation);
            Vector3 newForce = (slopeAngleRotation * force);
            Debug.DrawRay(downRay.position, newForce * 5,c,0.5f);
            Debug.Log("New Force " + newForce );
           // transform.rotation = slopeAngleRotation;
           // force.y *= -1;
        // }
        
    }
}
