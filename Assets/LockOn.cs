using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class LockOn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CinemachineBrain brain;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] CinemachineVirtualCamera lockOnCam;
    [SerializeField] private int enemyRadius;
    [SerializeField] private Canvas cursorCanvas;
    private LayerMask enemyMask;
    private PlayerControls controls;
    private bool lockOnPressed = false;
    private bool r1, l1;
    private Transform currentEnemy = null;
    private List<Transform> enemiesInRage;
    
    private int cursor = 0;
    void Start()
    {
       
        enemyMask = LayerMask.NameToLayer("Enemy");
        cinemachineTargetGroup.m_Targets[0].target = this.transform;
        controls = new PlayerControls();
        
        controls.Enable();
        controls.GroundMove.L1.performed += HandleL1;
        controls.GroundMove.L1.canceled  += HandleL1;
        controls.GroundMove.R1.performed += HandleR1;
        controls.GroundMove.R1.canceled  += HandleR1;
        controls.GroundMove.MoveCursor.performed += HandleCursorMove;
        controls.GroundMove.MoveCursor.canceled += HandleCursorMove;
        controls.WaterMove.Lockon.performed += HandleLockOn;
        PlayerEventPublisher.lockEvent += OnLockOn;

        enemiesInRage = new List<Transform>();



    }

    private void FixedUpdate()
    {
        if (currentEnemy != null)
        {
            cursorCanvas.transform.LookAt(new Vector3(this.transform.position.x,cursorCanvas.transform.position.y,this.transform.position.z));
            transform.LookAt(new Vector3(currentEnemy.position.x,this.transform.position.y,currentEnemy.position.z));
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (lockOnPressed)
        {
            cursorCanvas.enabled = false;
            cinemachineFreeLook.LookAt = this.transform;
            currentEnemy = null;
            cinemachineFreeLook.Priority = 10;
            lockOnCam.Priority = 1;
            lockOnPressed = false;
         //  brain.m_BlendUpdateMethod = CinemachineBrain.BrainUpdateMethod.LateUpdate;
            lockOnPressed = !lockOnPressed;
        }
        
        LockOnToTarget();
       
      
    }
    //Cycle through list of enemies and add them to list
    //TODO: Check to see if the current target is further than the enemyRadius to the player and remove if so
    
    void LockOnToTarget()
    { 
         Debug.Log("Lock on pressed " + lockOnPressed);
        if(lockOnPressed)
        {
            //enemiesInRage = new List<Transform>();
            Collider[] enemies = Physics.OverlapSphere(this.transform.position, enemyRadius, 1<<9);
            Debug.Log("enemies " + enemies.Length);
            if (enemies.Length == 0)
            {
               removeLockOn();
                return;
            }
            UpdateEnemyList(enemies);
            cursorCanvas.enabled = true;
            lockOnCam.Priority = 10;
            cinemachineFreeLook.Priority = 1;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemiesInRage.Contains(enemies[i].transform))
                {
                    enemiesInRage.Add(enemies[i].transform);
                }
            }
            if(currentEnemy == null)
            {
                lockOnCam.LookAt = enemiesInRage[cursor].transform;
                currentEnemy = enemiesInRage[cursor].transform;
                cursorCanvas.transform.position = currentEnemy.position;
            }
        }
    }
    //Write a better sorting algorithim eventually
    void SortEnemiesClosest()
    {
        for (int i = 0; i < enemiesInRage.Count;i++)
        {
            Transform enemy1 = enemiesInRage[i];
            float d1 = Vector3.Distance(enemy1.transform.position, this.transform.position);
            for (int j = i + 1; j < enemiesInRage.Count; j++)
            {
                Transform enemy2 = enemiesInRage[j];
                float d2 = Vector3.Distance(enemy2.transform.position, this.transform.position);
                if (d2 > d1)
                {
                    Transform temp = enemy1;
                    enemiesInRage[i] = enemy2;
                    enemiesInRage[j] = temp;
                    // //make sure we dont move the players currently targeted enemy
                    // if (currentEnemy == enemiesInRage[i]) cursor = i;
                    // if (currentEnemy == enemiesInRage[j]) cursor = j;
                }
            }
        }
    }

    void UpdateEnemyList(Collider[] enemies)
    {
        if(enemiesInRage.Count > 0)
        {
            //remove any enemies outside the lock on radius
            for (int i = enemiesInRage.Count-1; i >= 0; i--)
            {
                float d = Vector3.Distance(this.transform.position, enemiesInRage[i].position);
                if (d > enemyRadius)
                {
                    //swap current enemy
                    if (cursor == i)
                    {
                        cursor++;
                        if (cursor >= enemiesInRage.Count)
                        {
                            cursor = 0;
                        }
                        currentEnemy = enemiesInRage[cursor];
                    }
                    enemiesInRage.Remove(enemiesInRage[i]);
                }
            }
        }
        
        if(enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Transform t = enemies[i].transform;
                if (!enemiesInRage.Contains(t))
                {
                    enemiesInRage.Add(t);
                    SortEnemiesClosest();
                }
            }
        }
    }
    
    void HandleL1(InputAction.CallbackContext context)
    {
        l1 = context.performed;
    }
    void HandleR1(InputAction.CallbackContext context)
    {
       
        r1 = context.performed;
    }

    void HandleLockOn(InputAction.CallbackContext context)
    {
        Debug.Log("Lock On " + context.performed);
        if (context.performed)
        {
            lockOnPressed = !lockOnPressed;
        }

        if (lockOnPressed == false)
        {
            removeLockOn();
        }
    }

    void removeLockOn()
    {
        enemiesInRage = new List<Transform>();
        cursorCanvas.enabled = false;
        cinemachineFreeLook.LookAt = this.transform;
        currentEnemy = null;
        cinemachineFreeLook.m_Lens = lockOnCam.m_Lens;
        cinemachineFreeLook.Priority = 10;
        lockOnCam.Priority = 1;
        lockOnPressed = false;
    }

    void HandleCursorMove(InputAction.CallbackContext context)
    { 
        Vector2 dpad  = context.ReadValue<Vector2>();
        if(enemiesInRage.Count > 1)
        {
            cursor += (int)dpad.x;
            if (cursor > enemiesInRage.Count) cursor = 0;
            if (cursor < 0) cursor = enemiesInRage.Count - 1;
        }
    }

    void OnLockOn(object source, bool lockOn)
    {
        Debug.Log("Lock on " + lockOn);
        lockOnPressed = lockOn;
        if (lockOnPressed == false)
        {
            removeLockOn();
        }
        
    }

    public GameObject GetTarget()
    {
        if (currentEnemy != null)
        {
            return currentEnemy.gameObject;
        }

        return null;
        
    }

    private void OnDestroy()
    {
        controls.GroundMove.L1.performed -= HandleL1;
        controls.GroundMove.L1.canceled  -= HandleL1;
        controls.GroundMove.R1.performed -= HandleR1;
        controls.GroundMove.R1.canceled  -= HandleR1;
        controls.GroundMove.MoveCursor.performed -= HandleCursorMove;
        controls.GroundMove.MoveCursor.canceled  -= HandleCursorMove;
        PlayerEventPublisher.lockEvent -= OnLockOn;
        controls.Disable();
    }
}
