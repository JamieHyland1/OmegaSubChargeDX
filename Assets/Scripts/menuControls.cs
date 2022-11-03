using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuControls : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    [SerializeField]
    Animator animator;
    Vector3 xRotation;
    bool rising = false;
    bool falling = false;
     float rise;
    float fall;
     Rigidbody rigidbody;

     void Awake()
    {
       controls = new PlayerControls();
      Debug.Log( controls.Move.Rise);
     
    }


    // Start is called before the first frame update
    void Start()
    {
        controls = new PlayerControls();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rise = controls.Move.Rise.ReadValue<float>();
        fall = controls.Move.Fall.ReadValue<float>();
        
        move = controls.Move.Turn.ReadValue<Vector2>();

        Debug.Log("move " + move + " rise " + rise + " fall " + fall);

        animator.SetBool("Rising", rising);
        animator.SetBool("Falling", falling);

        xRotation = new Vector3(0, 200 * move.x, 0);

    }

      void FixedUpdate()
    {
        Quaternion deltaRotation =  Quaternion.Euler(xRotation * Time.fixedDeltaTime * Time.timeScale);           
        rigidbody.MoveRotation(deltaRotation * rigidbody.rotation);
      
       
        
    }

}
