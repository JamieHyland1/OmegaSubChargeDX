using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CamControllerTest : MonoBehaviour
{

    public CinemachineFreeLook cinemachineFreeLook;
    // Start is called before the first frame update
    void Start()
    {
        cinemachineFreeLook.m_Orbits[0].m_Radius = 25;
        cinemachineFreeLook.m_Orbits[0].m_Height = 22;
        cinemachineFreeLook.m_Orbits[1].m_Radius = 35;
        cinemachineFreeLook.m_Orbits[1].m_Height = 12;
        cinemachineFreeLook.m_Orbits[2].m_Radius = 25;
        cinemachineFreeLook.m_Orbits[2].m_Height = 0;
        // cinemachineFreeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = f;
        // cinemachineFreeLook.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenX = f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
