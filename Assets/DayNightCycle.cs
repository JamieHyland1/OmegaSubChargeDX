using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Material skybox;
    [SerializeField] private Material water;

    [SerializeField]private Gradient skyGradient;
    [SerializeField]private Gradient waterGradient1;
    [SerializeField]private Gradient waterGradient2;

    [Range(0,360)]private float rotation;
    [SerializeField]private float currentTime;

    [SerializeField] private float dayLength = 10;
    // Start is called before the first frame update
    void Start()
    {
        skybox.SetFloat("_Exposure",0.43f);
        RenderSettings.skybox = skybox;

        // skybox.SetColor(, Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", (currentTime/dayLength)*360);
        RenderSettings.skybox.SetColor("_Tint", skyGradient.Evaluate(currentTime/dayLength));
        water.SetColor("_Col2", waterGradient1.Evaluate(currentTime/dayLength));
        water.SetColor("_Col1", waterGradient2.Evaluate(currentTime/dayLength));
        if (currentTime / dayLength > 1) currentTime = 0;
    }
}
