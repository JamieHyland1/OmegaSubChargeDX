using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflections : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material skybox;
    [SerializeField] private Material water;
    public Color skyTint;
    void Start()
    {
        // skybox.SetColor("Tint Color",skyTint);
    }

    // Update is called once per frame
    void Update()
    {
        //skyTint = (skybox.GetColor("Tint Color"));
       skyTint = RenderSettings.skybox.GetColor("_Tint");
       water.SetColor("Skybox Color",skyTint);
        
    }
}
