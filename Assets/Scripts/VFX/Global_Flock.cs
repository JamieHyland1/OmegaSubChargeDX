using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Global_Flock : MonoBehaviour
{
    public GameObject fishPrefab;

    public int numberOfFish = 40;


    public float tankSizeX;
    public float tankSizeY;
    public float tankSizeZ;
    public GameObject[] allFish;

    public Vector3 goalPos = Vector3.zero;

    

    void Start()
    {
        allFish = new GameObject[numberOfFish];

        

        for (int i = 0; i < numberOfFish; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-tankSizeX, tankSizeX) + transform.position.x,
                                      Random.Range(-tankSizeY, tankSizeY) + transform.position.y,
                                      Random.Range(-tankSizeZ, tankSizeZ) + transform.position.z);
            allFish[i] = (GameObject) Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100) < 2){
            goalPos = new Vector3(Random.Range(-tankSizeX, tankSizeX) + transform.position.x,
                                  Random.Range(-tankSizeY, tankSizeY) + transform.position.y,
                                  Random.Range(-tankSizeZ, tankSizeZ) + transform.position.z);
            //Debug.Log(goalPos);
        }
    }
}
