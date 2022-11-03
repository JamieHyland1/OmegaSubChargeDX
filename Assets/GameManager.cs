using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] gems;
    int checkTimer = 10;
    float counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        counter = checkTimer;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if(counter <= 0){
            int numGems = GameObject.FindGameObjectsWithTag("Gem").Length;
            if(numGems <= 0){
                this.GetComponent<LevelLoader>().LoadLevel();
            }
            counter = checkTimer;
        }
    }
}
