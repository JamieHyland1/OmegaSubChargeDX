using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_billboard_y_axis : MonoBehaviour
{
    private void LateUpdate()
     {
         transform.forward = new Vector3(Camera.main.transform.forward.x, 
         transform.forward.y, Camera.main.transform.forward.z);
     }
}
