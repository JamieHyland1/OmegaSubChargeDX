using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform ledgeCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float ledgeCheckDistance;
    LayerMask groundLayer;
    void Start(){
        groundLayer = LayerMask.GetMask("Level Geometry");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit wallHit;
        RaycastHit ledgeHit;

        if(Physics.Raycast(wallCheck.position, -wallCheck.forward, out wallHit, wallCheckDistance, groundLayer)){
            Debug.Log("Hit wall");
            Debug.DrawRay(wallCheck.position, Vector3.forward, Color.green, 0.20f, true);
            // this.transform.position = new Vector3(wallHit.point.x, this.transform.position.y, wallHit.point.z-3);
        }else{
            Debug.DrawRay(wallCheck.position, Vector3.forward, Color.red, 0.20f, true);
        }
        
        if(Physics.Raycast(ledgeCheck.position, ledgeCheck.TransformDirection(Vector3.down), out ledgeHit, ledgeCheckDistance, groundLayer)){
            Debug.Log("Ledge detected");
            Debug.DrawRay(ledgeCheck.position, Vector3.down, Color.green, 0.20f, true);
            if(ledgeHit.distance <= 0.2f)this.transform.position = new Vector3(wallHit.point.x, ledgeHit.point.y-9, wallHit.point.z-2f);
        }else{
            Debug.DrawRay(ledgeCheck.position, Vector3.down, Color.red, 0.20f, true);
        }

    }
}
