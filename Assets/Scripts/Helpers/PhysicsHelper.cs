using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHelper 
{
    
    public float gravity = -85;
    public float gravityScale  = 1;
    float speed {get; set;}
    public float moveSpeed {get; set;}
    public float runSpeed {get; set;}
    public float afterDashSpeed {get; set;}
    public float jumpVelocity {get; set;}
    private Vector3 previousVelocity; 

    
    
    private PhysicsHelper() {}  
    private static PhysicsHelper instance = null;  
    public static PhysicsHelper Instance {  
        get {  
            if (instance == null) {  
                instance = new PhysicsHelper();  
            }  
            return instance;  
        }  
    }  
    
    public bool checkForSlopes(Transform playerTransform, CharacterController controller, float slopeCheckLength, float slopeAngleThreshold){
        Ray ray = new Ray(playerTransform.position, playerTransform.forward);
        RaycastHit hit;
        Physics.Raycast(ray,out hit, slopeCheckLength);
        if(hit.collider != null){
            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up,hit.normal);
            float radius = Mathf.Abs(5/Mathf.Sin(slopeAngle));
            if(slopeAngle >= slopeAngleThreshold){
                if(hit.distance - controller.radius > Mathf.Abs(Mathf.Cos(slopeAngle) * radius)){
                    return true;
                }
                return false;
            }
            return true;
        }
        return true;
    }
    public Vector3 applyGravity(Vector3 velocity, bool jumpButtonHeld){
        previousVelocity = velocity;
        Vector3 nextVelocity = velocity;
        if (!jumpButtonHeld) nextVelocity.y += (gravity * gravityScale) * Time.deltaTime;
        else nextVelocity.y += gravity * Time.deltaTime;

        float newYVelocity = Mathf.Max((previousVelocity.y + nextVelocity.y) * 0.5f,-20.0f);
        velocity.y = newYVelocity;
        return velocity;
    }

    public bool checkPos(Vector3 checkPos, float radius, LayerMask mask){
        return Physics.CheckSphere(checkPos,radius,mask);
    }

    // public void rayCastHit(Vector3 startPos,Vector3 direction,float rayLength, out RaycastHit hit, LayerMask layer){
    //     Physics.Raycast(new Ray(startPos,direction),hit,rayLength);
    // }
    public Vector3 slideDownSlope(Vector3 velocity, Vector3 groundCheck){
        Ray ray = new Ray(groundCheck, Vector3.down);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 5);
        if(hit.collider != null){
            velocity.x += (1f - hit.normal.y) * hit.normal.x * (-gravity - 0.5f);
            velocity.z += (1f - hit.normal.y) * hit.normal.z * (-gravity - 0.5f);
        }
        return velocity;
    }
    public RaycastHit getHitInfo(Vector3 startPos, Vector3 direction, float rayLength,LayerMask layer){
        Ray ray = new Ray(startPos, direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, rayLength, layer);
        return hit;
    }


    public void rotateCharacter(Vector3 direction, Transform playerTransform, Transform cam, float turnSmoothTime, float turnSmoothVelocity, out Vector3 moveDirection, out float angle){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            playerTransform.rotation = Quaternion.Euler(0, angle, 0);
            moveDirection = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
    }


    public bool checkGroundCollision(bool isGrounded, Transform groundCheck,LayerMask groundLayer){
            isGrounded = (Physics.CheckSphere(groundCheck.position,0.1f,groundLayer,QueryTriggerInteraction.Ignore));
           
            return isGrounded;
        }


        public float Lerp3(float a, float b, float c, float t){
            if(t <= 0.5f){
                return Mathf.Lerp(a,b,t);
            }
            return Mathf.Lerp(b,c,t);
        }
}
