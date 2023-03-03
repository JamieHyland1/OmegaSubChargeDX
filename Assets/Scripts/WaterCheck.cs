
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
    LayerMask waterLayer;
    // Start is called before the first frame update
    void Start()
    {
        waterLayer  = LayerMask.GetMask("Water");
    }

    // Update is called once per frame
    void Update()
    {
       if(Physics.OverlapSphere(this.transform.position, 0.5f, waterLayer).Length > 0)mat.SetColor("_WaterTint",water); else mat.SetColor("_WaterTint",Color.black) ;
    }
}
