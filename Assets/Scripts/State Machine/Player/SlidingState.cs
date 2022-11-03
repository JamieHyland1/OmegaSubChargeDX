
using System.Security.AccessControl;
using Unity;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;

    



    public class SlidingState : IState
    {
        string STATE_NAME = "Slide State";
        PlayerSM playerSM;
        Transform playerTransform;
        Transform groundCheck;
        Transform cam;
        CharacterController controller;
     
       // PhysicsHelper helper;
        PlayerControls controls;
        Vector2 move;
        Vector3 direction;
        Vector3 moveDirection;
        float turnSmoothTime;
        float turnSmoothVelocity;
        float speed;
        float maxSpeed = 200;

        float slideTimer = 1;
        float counter = 0;
        Vector3 velocity;
        public SlidingState(PlayerSM playerSM, Transform playerTransform, Transform groundCheck, Transform cam, ref CharacterController controller, PlayerControls controls, float turnSmoothTime, float turnSmoothVelocity){
            this.playerSM = playerSM;
            this.playerTransform = playerTransform;
            this.groundCheck = groundCheck;
            this.cam = cam;
            this.controller = controller;
            this.controls = controls;
            
        }

        public void Enter(){
           //change to eventManager method
           // animator.SetBool("IsSliding", true);  
            EventManager.current.OnPlayerTriggerSlidingUpdate(true);
            move = new Vector2();
            controller.height = 4f;
            controller.radius = 2;
            controller.center = new Vector3(0,2f,-1);  
            controls.Sliding.Enable();
            controls.Sliding.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
            controls.Sliding.Move.canceled  += ctx => move = Vector2.zero;
            controls.Sliding.Slide.started += ctx => playerSM.ChangeState(playerSM.moveState);
            velocity = playerSM.getVelocity();
            speed = playerSM.getCurrentSpeed();
            direction = playerSM.getDirection();
            maxSpeed = speed * 1.25f;
            Debug.Log("WE UP OUT HERE BOIZ");
            counter = slideTimer;
        }
        public void FixedTick(){
        
        }

        public void Tick(){
            counter -= Time.deltaTime;
            playerSM.isGrounded = PhysicsHelper.Instance.checkGroundCollision(playerSM.isGrounded,groundCheck,playerSM.groundLayer);
            if(!playerSM.isGrounded)velocity = PhysicsHelper.Instance.applyGravity(velocity,true);
             Debug.Log("speed " + speed + " counter " + counter);
            if(counter <= 0){
                if(speed > 0)speed -= 0.25f;else playerSM.ChangeState(playerSM.moveState);
                Debug.Log("speed " + speed);
            }
            RaycastHit hit = PhysicsHelper.Instance.getHitInfo(playerSM.groundCheck.position,Vector3.down,5f, playerSM.groundLayer);
            direction = new Vector3(move.x, 0f, move.y);
            float angle = 0;
            moveDirection = new Vector3();
            PhysicsHelper.Instance.rotateCharacter(direction,playerTransform,cam,0.025f,0, out moveDirection, out angle);
            velocity.x = moveDirection.x * speed;
            velocity.z = moveDirection.z * speed;
            if(speed < maxSpeed)speed += 0.095f;
            Debug.Log(velocity.magnitude > maxSpeed);
            Debug.Log(velocity.magnitude + " " + maxSpeed);
            if(velocity.magnitude > maxSpeed){
                Vector3 clampedVel = Vector3.ClampMagnitude(new Vector3(velocity.x,0,velocity.z),maxSpeed);
                velocity = clampedVel;
            }
            

//                 if(hit.normal == Vector3.up)hit.normal = playerTransform.forward;
// //                Debug.Log("Hit " +  hit.normal);
//                 velocity.x += (2f - hit.normal.y) * hit.normal.x * (-helper.gravity - 0.005f);
//                 velocity.z += (2f - hit.normal.y) * hit.normal.z * (-helper.gravity - 0.005f);
            controller.Move(velocity * Time.deltaTime);
        }


        public void Exit(){
            Debug.Log("leaving Sliding state");
            playerSM.setCurrentSpeed(speed);
            playerSM.setVelocity(velocity);
            playerSM.setDirection(direction);
            controls.Sliding.Disable();
           //set to eventmanager method
            EventManager.current.OnPlayerTriggerSlidingUpdate(false);
            // animator.SetTrigger("ExitSlide");

        }


        public void PrintStateName(){
            Debug.Log(STATE_NAME);
        }
        public void EventTrigger(){}
    }

