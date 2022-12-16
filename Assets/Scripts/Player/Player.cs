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
    Vector2 mouseMove;
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
       speed = moveSpeed;
       angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hit, Mathf.Infinity, layerMask)){
          
        }
        if(counter <= 0) isDashing = false;

        move = controls.Move.Turn.ReadValue<Vector2>();
        throttle = controls.Move.Accelerate.ReadValue<float>();
        xRotation = new Vector3(0,200 * move.x,0);
        rise = controls.Move.Rise.ReadValue<float>();
        fall = controls.Move.Fall.ReadValue<float>();
        // Debug.Log("fall " + fall);
        mouseMove = (controls.Move.Rotate.ReadValue<Vector2>());

        if (rise + -fall > 0)  rising  = true; else  rising = false;
        if (rise + -fall < 0) falling = true; else falling = false;

       if(!inMenu){

        moveDirection = transform.forward;

        if(hit.distance <= 2.8f && !inMenu && !isDashing){

            rise = 0;

            rigidbody.velocity = new Vector3(rigidbody.velocity.x,0,rigidbody.velocity.z);
        }
     
        yVector.y = (rise + -fall);
       
        if(manager.enabled)turbo = manager.boostSlider.value;

        if(controls.Move.Boost.ReadValue<float>() > 0 && turbo > 0){

            boostEffectObj.SetActive(true);

        }
            else boostEffectObj.SetActive(false); 
        }

        if(!inMenu){
        if(controls.Move.Attack.ReadValue<float>() > 0){
            animator.SetTrigger("Boost"); 
        }
       
            // else if(controls.Move.Boost.ReadValue<float>() < 1)manager.boostSlider.value += Time.deltaTime/15;
            // if(controls.Move.Boost.ReadValue<float>() > 0 && manager.boostSlider.value > 0) speed = boostSpeed; else speed = moveSpeed;
        }





        mouseMove = mouseMove.normalized;
        moveDirection = moveDirection.normalized;
        
        if(!isDashing){
            xRotation = new Vector3(0, 200 * move.x, 0);
        }


        

        animator.SetBool("Rising",  rising);
        animator.SetBool("Falling", falling);

        counter -= Time.deltaTime;
    }



    void FixedUpdate()
    {

        var camera = Camera.main;

        // Y rotation based on camera
        float targetAngle = Mathf.Atan2(move.x, 0) * Mathf.Rad2Deg + camera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
               
        if(!isDashing){
            
            rigidbody.MoveRotation(Quaternion.Euler(0,angle  * Time.timeScale,0));
            rigidbody.AddForce((Vector3.up * ySpeed * (rise + -fall)) +  (transform.forward * speed * throttle));
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
