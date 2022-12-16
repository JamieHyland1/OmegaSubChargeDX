using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{

    [SerializeField]
    LayerMask mask;
    [SerializeField]
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void Update()
    {
        if (Physics.CheckSphere(transform.position, 15, mask)){
            transform.position = Vector3.MoveTowards(transform.position, player.position, 2.5f);
        }
    }
      private void LateUpdate()
     {
         transform.forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
     }
}
