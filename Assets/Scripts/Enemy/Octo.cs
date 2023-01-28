// using UnityEngine;

// public class Octo : MonoBehaviour
// {
    
//     [SerializeField]
//     int checkRadius = 250;
//     [SerializeField]
//     Rigidbody rigidbody;
//     [SerializeField]
//     LayerMask mask;

//     [SerializeField]
//     LayerMask layerMask;

//     Transform closestGem;
//     float d = 500000;

//     public GameObject[] gems;

//     void Start()
//     {
//         gems = GameObject.FindGameObjectsWithTag("Gem");
//         closestGem = gems[0].transform;
//         for(int i = 0; i < gems.Length; i++){
//             float currentD = Vector3.Distance(closestGem.position,gems[i].transform.position);
//             if(currentD < d)closestGem = gems[i].transform;
//         }
//         rigidbody.AddForce(Vector3.down * 25 , ForceMode.Impulse);
//     }

//     void Update(){

//          RaycastHit hit;


//          if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask) && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
//             Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
//             //Debug.Log("Octo not under water");
//             rigidbody.useGravity  = true;

//         }else{
//           rigidbody.useGravity  = false;

//         //  if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask)){
//         //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
//         //     Debug.Log("Octo under water");
//         // }



//             if (Physics.CheckSphere(transform.position, checkRadius,mask)){

                
//                 // Determine which direction to rotate towards
//                 Vector3 targetDirection = closestGem.transform.position - transform.position;

//                 // The step size is equal to speed times frame time.
//                 float singleStep = 5 * Time.deltaTime;

//                 // Rotate the forward vector towards the target direction by one step
//                 Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0, 0.0f);

        

//                 // Calculate a rotation a step closer to the target and applies rotation to this object
//                 transform.rotation = Quaternion.LookRotation(newDirection);

//                 transform.position = Vector3.MoveTowards(transform.position, closestGem.position,10f*Time.deltaTime);
//             } 
//         }
//     }
// }
