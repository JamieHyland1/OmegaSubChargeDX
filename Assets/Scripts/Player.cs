using System;
using UnityEngine;
public class Player : MonoBehaviour
{
    PlayerControls controls;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float turnSmoothVelocity;
    [SerializeField]
    float turnSmoothTime;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float boostSpeed;
    [SerializeField]
    float ySpeed;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Material attackMat;
    [Range(0,1),SerializeField]
    public static float turbo = 1;
    [SerializeField]
    UIManager manager;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    GameObject boostEffectObj;
    [SerializeField]
    float fallTriggerDeadzone;
    [SerializeField]
    float dashSpeed;

     [SerializeField]
    int boostTime = 1;
    [SerializeField]
    bool inMenu = false;


    float throttle = 0;
    float rise;
    float fall;
    float dashTimer = 1.25f;
    bool isDashing = false;
    bool isBoosting = false;
    float counter = 0;
    float speed;
    
    Rigidbody rigidbody;
    Vector2 move;
    Vector3 xRotation;
    Vector3 yRotation;
    Vector3 yVector;

    Vector3 moveDirection;
    float angle;
    float targetAngle;

    bool rising = false;
    bool falling = false;
    // Start is called before the first frame update
    void Awake()
    {
       controls = new PlayerControls();
       yVector = new Vector3();
     
    }
    void Start()
    {
       move = new Vector2();
       rigidbody = GetComponent<Rigidbody>();
       Debug.Log(manager);
       
       angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;


         if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask)){
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow);
            //Debug.Log( "distance " + hit.distance);
        }
        if(counter <= 0) isDashing = false;

        move = controls.Move.Turn.ReadValue<Vector2>();
        throttle = controls.Move.Accelerate.ReadValue<float>();
        xRotation = new Vector3(0,200 * move.x,0);
        rise = controls.Move.Rise.ReadValue<float>();
        fall = controls.Move.Fall.ReadValue<float>();



        if (rise + -fall > 0)  rising  = true; else  rising = false;
        if (rise + -fall < 0) falling = true; else falling = false;

       if(!inMenu){
        moveDirection = transform.forward;

        if(hit.distance <= 3f && !inMenu && !isDashing){
            rise = 0;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
        }
        if (fall < fallTriggerDeadzone) fall = 0;
        yVector.y = (rise + -fall);
       
        // Debug.Log("Rise " + rise + " fall " + fall);
        if(manager.enabled)turbo = manager.boostSlider.value;
        if(controls.Move.Boost.ReadValue<float>() > 0 && turbo > 0){
            manager.boostSlider.value -= Time.deltaTime/boostTime;
            boostEffectObj.SetActive(true);

       }
       else boostEffectObj.SetActive(false); 
       }
        if(!inMenu){
        if(controls.Move.Attack.ReadValue<float>() > 0){
            animator.SetTrigger("Boost"); 
        }
       
            else if(controls.Move.Boost.ReadValue<float>() < 1)manager.boostSlider.value += Time.deltaTime/15;
            if(controls.Move.Boost.ReadValue<float>() > 0 && manager.boostSlider.value > 0) speed = boostSpeed; else speed = moveSpeed;
        }







        moveDirection = moveDirection.normalized;
        if(!isDashing){
            xRotation = new Vector3(0, 200 * move.x, 0);
        }


        

        animator.SetBool("Rising", rising);
        animator.SetBool("Falling", falling);


            

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
     

        //-183 , -13.6, -95.39

        counter -= Time.deltaTime;
    }



    void FixedUpdate()
    {
        Quaternion deltaRotation =  Quaternion.Euler(xRotation * Time.fixedDeltaTime * Time.timeScale);           
        if(!isDashing){
            rigidbody.MoveRotation(deltaRotation * rigidbody.rotation);
            rigidbody.AddForce((Vector3.up * ySpeed * (rise + -fall))  + (moveDirection * speed * throttle));
            counter = dashTimer;
        }
       
        
    }

    void OnEnable(){

        controls.Enable();
    }

    void OnDisable(){
        controls.Disable();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DashRing"){
            isDashing = true;
            this.transform.position = other.transform.position;
            animator.SetTrigger("Dash");
            rigidbody.velocity = new Vector3();
           if(!inMenu) rigidbody.AddForce(other.transform.forward * dashSpeed, ForceMode.VelocityChange);
            rigidbody.rotation = other.transform.rotation;
        }
        if(other.gameObject.tag == "Anchor"){
            Destroy(other.gameObject);
           manager.boostSlider.value += Time.deltaTime*2;
            
            manager.anchorCount ++;
        }
       
    }
}
