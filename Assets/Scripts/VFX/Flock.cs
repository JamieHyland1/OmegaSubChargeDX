using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float speed = 0.1f;
    public float maxSpeed;
    [SerializeField]
    float rotationSpeed = 4f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    [SerializeField]
    float neighborDistance = 2f;
    public float randomSpeedMin;
    public float randomSpeedMax;
    public float avoidDistance;
    public float randomDirectionAmt = 1.0f;
    Vector3 addRandom;
    bool beenSpooked = false;
    public float spookedSpeed;
    public float spookTime = 2f;
    private SphereCollider sphereCol;
    Animator anim;
    public GameObject fishManager;
    private Global_Flock globalFlock;

    bool turning = false;

    private void Awake() {
        sphereCol = this.GetComponent<SphereCollider>();
        anim = this.GetComponent<Animator>();
        
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("FishManager");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos){
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance){
                closest = go;
                distance = curDistance;
            }
        }
        fishManager = closest;
        globalFlock = fishManager.GetComponent<Global_Flock>();
    }
    
    void Start()
    {
        speed = Random.Range(randomSpeedMin, randomSpeedMax);
    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.gameObject.CompareTag("Player")){
            beenSpooked = true;  
            speed = spookedSpeed;  
            StartCoroutine(SpookCountdown());
        }
        
    }

    

    private IEnumerator SpookCountdown(){
        
        yield return new WaitForSeconds(spookTime);
        speed = Random.Range(randomSpeedMin, randomSpeedMax);
        beenSpooked = false;
    }

    
    void Update()
    {
        anim.speed = speed/8f;

        if (Random.Range(0, 100) < 1){
            addRandom = new Vector3(Random.Range(-randomDirectionAmt, randomDirectionAmt), 
                                    Random.Range(-randomDirectionAmt, randomDirectionAmt), 
                                    Random.Range(-randomDirectionAmt, randomDirectionAmt));

        }

        if (!beenSpooked){
            if (speed > maxSpeed){
                speed = Random.Range(randomSpeedMin, randomSpeedMax);
            }
            if(Mathf.Abs(transform.position.x - fishManager.transform.position.x) >= globalFlock.tankSizeX ||
               Mathf.Abs(transform.position.y - fishManager.transform.position.y) >= globalFlock.tankSizeY ||
               Mathf.Abs(transform.position.z - fishManager.transform.position.z) >= globalFlock.tankSizeZ){
                turning = true;
            }
            else
            {
                turning = false;
            }
            if (turning)
            {
                Vector3 direction = fishManager.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
                speed = Random.Range(randomSpeedMin, randomSpeedMax);
            }
            else
            {
                if(Random.Range(0,5) < 1);
                    ApplyRules();
            }
        }
        //for turning away from the player
        if (beenSpooked)
        {
            GameObject other = GameObject.Find("Player");
            Vector3 playerPos = other.transform.position - transform.position;
            Quaternion playerLook = Quaternion.LookRotation(playerPos.normalized);
            //Debug.Log(playerLook.eulerAngles);
            Quaternion turnOneEighty = Quaternion.Inverse(playerLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, turnOneEighty, (rotationSpeed * 3f) * Time.deltaTime);
        }
        
        transform.Translate(0,0,Time.deltaTime*speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = globalFlock.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.1f;

        Vector3 goalPos = globalFlock.goalPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if(go != this.gameObject) {
                {
                    dist = Vector3.Distance(go.transform.position,this.transform.position);
                    if(dist <= neighborDistance)
                    {
                        vcentre += go.transform.position;
                        groupSize++;
                        
                        if(dist < avoidDistance){
                            vavoid = vavoid + (this.transform.position - go.transform.position);
                        }
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre/groupSize + (goalPos - this.transform.position);
            speed = Mathf.Clamp(gSpeed/groupSize, 0f, maxSpeed);

            Vector3 direction = (vcentre + vavoid) - transform.position;

            direction += addRandom;      

            if(direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }
}
