using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugHelper : MonoBehaviour
{
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform slopeCheck;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask layer;
    PhysicsHelper helper;
    Vector3[] floorChecks = new Vector3[3];
    bool[] floorBools = new bool[3];
    bool[] slopeBools = new bool[3];

    void Awake(){
    }
    void Update(){
        floorChecks[0] = groundCheck.position - new Vector3(controller.radius/2,0,0);
        floorChecks[1] = groundCheck.position;
        floorChecks[2] = groundCheck.position + new Vector3(controller.radius/2,0,0);
        //slopeCheck.transform.rotation = this.transform.rotation;
        // for(float i = -90 * Mathf.Deg2Rad; i <= 90 * Mathf.Deg2Rad; i+=0.1f){
        //     slopeCheck.rotation = transform.rotation * Quaternion.Euler(0f, i * Mathf.Rad2Deg, 0f);
        //     if(!checkForSlopes()){
        //         Debug.Log("hit steep slope.....shit");
        //         Debug.DrawRay(this.slopeCheck.position, slopeCheck.forward * 5, Color.red, 0.5f);
        //         break;
        //     } else{
        //          Debug.Log("Havent found a slope yet...");
        //              Debug.DrawRay(this.slopeCheck.position, slopeCheck.forward * 5, Color.blue, 0.5f);
        //     } 
        // }

        Debug.DrawRay(this.transform.position + Vector3.up * controller.height/2,transform.forward*5,Color.black,1f);
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position + Vector3.up * controller.height/2,transform.forward*5);
        Physics.Raycast(ray,out hit, 5, layer);
        if(hit.collider != null){
            Vector3 vel = hit.point;
            vel =   transform.rotation * Quaternion.Euler(0f, Mathf.Sin(Time.deltaTime)* 45 * Mathf.Rad2Deg, 0f).eulerAngles;
            Debug.DrawRay(hit.point, vel*-15 ,Color.red,1f);
        }
        slopeBools[0] = helper.checkForSlopes(this.transform,controller,5,0.2f);
        slopeBools[1] = helper.checkForSlopes(this.transform,controller,5,0.2f);
        slopeBools[2] = helper.checkForSlopes(this.transform,controller,5,0.2f);
    } 
    void OnDrawGizmos(){
        if(Application.isPlaying){
            //Ground Check
            
                floorBools[0] = Physics.CheckSphere(floorChecks[0],0.5f,layer);
                floorBools[1] = Physics.CheckSphere(floorChecks[1],0.5f,layer);
                floorBools[2] = Physics.CheckSphere(floorChecks[2],0.5f,layer);

               
                
                if(floorBools[0] && (floorBools[1] && floorBools[2])){
                    Gizmos.color = Color.green;
                }else{
                    Gizmos.color = Color.red;
                }
            for(int i = 0; i < floorChecks.Length; i++){
                Gizmos.DrawSphere(floorChecks[i],0.1f);
                // Debug.Log(floorChecks[i]);
                if(slopeBools[i] && (slopeBools[1] && slopeBools[2]))
                    Debug.DrawLine(floorChecks[i] + Vector3.up*controller.height/4, floorChecks[i] + this.transform.forward*0.5f,Color.red);
                else
                    Debug.DrawLine(floorChecks[i] + Vector3.up*controller.height/4, floorChecks[i] + this.transform.forward*0.5f,Color.green);
                
            }
        }

       
    }
     bool checkForSlopes(){
            Ray ray = new Ray(slopeCheck.position, new Vector3(slopeCheck.forward.x,0,slopeCheck.forward.z));
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 2.5f);
           
            if(hit.collider != null){
                 Debug.DrawRay(slopeCheck.position,slopeCheck.forward * 5,Color.black, 1f);
                float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up,hit.normal);
                float radius = Mathf.Abs(1/Mathf.Sin(slopeAngle));
              //  Debug.Log("Slope Angle" + " " + slopeAngle + " radius " + radius + " slopeAngleThreshold " + slopeAngleThreshold);
                if(slopeAngle >= 15 * Mathf.Deg2Rad){
                    if(hit.distance - controller.radius > Mathf.Abs(Mathf.Cos(slopeAngle) * radius)){
                        return true;
                    }
                    return false;
                }
                return true;
            }
            return true;
        }
}
