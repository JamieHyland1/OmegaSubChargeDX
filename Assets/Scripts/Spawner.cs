using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float spawnRadius = 1;

    [SerializeField]
    float innerRadius = 1;
   
    [SerializeField]
    GameObject obj;

    [SerializeField, Header("Minutes")]
    float spawnCycle = 1;

    [SerializeField, Range(1,50)]
    int numberToSpawn = 1;

    float counter = 0;

    bool spawning = false;

    void Start()
    {
        spawnCycle *= 60;
        counter = spawnCycle;
        Debug.Log("number to spawn " + numberToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
       
        counter -= Time.deltaTime;
        if(counter <= 0 && !spawning){
            spawning = true;
            StartCoroutine(spawnObjs());
        }
        
    }


    public IEnumerator spawnObjs(){
        for(int i = 0; i < numberToSpawn; i++){
            
            Vector2 r = Random.insideUnitCircle * spawnRadius;
            Vector3 pIC = new Vector3(transform.position.x + r.x,transform.position.y,transform.position.z + r.y);
            while(Vector3.Distance(pIC, transform.position) < innerRadius){
                r = Random.insideUnitCircle * spawnRadius;
                pIC = new Vector3(transform.position.x + r.x,transform.position.y,transform.position.z + r.y);
            }
        
            Vector3 spawnPoint = new Vector3(pIC.x, transform.position.y, pIC.z);
            GameObject octo = Instantiate(obj, spawnPoint,Quaternion.identity,this.transform);
            octo.transform.localScale  = new Vector3(8,8,8);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        counter = spawnCycle;
        spawning = false;
    }

   
}