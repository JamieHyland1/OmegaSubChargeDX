using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject submarine;
    void Start()
    {
        
    }

    // Update is called once per frame
   
    public void MechToSub(){
        Debug.Log("jamie ");
        submarine.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void SubToMech(){
         Debug.Log("jamie ");
        submarine.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
