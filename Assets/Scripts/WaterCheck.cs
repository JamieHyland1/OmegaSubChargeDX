
using System.Collections.Generic;
using UnityEngine;

public class WaterCheck : MonoBehaviour
{
    [SerializeField]
    Material mat;
    [SerializeField]
    Transform waterPoint;
    [SerializeField]
    Transform cam;
    [SerializeField]
    Color water;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(cam.position.y < waterPoint.position.y)mat.SetColor("_Tint",water); else mat.SetColor("_Tint",Color.white) ;
    }
}
