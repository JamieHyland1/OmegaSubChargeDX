using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Debug.Log("Magnitude " + new Vector3(-0.002591332f,-0.002591332f,-0.002591332f ).magnitude);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.position = cam.gameObject.transform.position;
        this.transform.rotation = cam.gameObject.transform.rotation;
    }
}
