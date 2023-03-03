using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerCameraEventListener : MonoBehaviour
{
    // Start is called before the first frame update
        [SerializeField] CinemachineFreeLook cinemachineFreeLook;
        
        [Header("Sub Camera Settings")]
        
        [Header("Height Settings")]
        [SerializeField] float sub_top_height = 40;
        [SerializeField] float sub_middle_height = 16;
        [SerializeField] float sub_bottom_height = -20;
        
        [Header("Radius Settings")]
        [SerializeField] float sub_top_radius = 25;
        [SerializeField] float sub_middle_radius = 50;
        [SerializeField] float sub_bottom_radius = 15;

        [Header("Mech Camera Settings")]
        [Header("Height Settings")]
        [SerializeField] float mech_top_height = 45;
        [SerializeField] float mech_middle_height = 20;
        [SerializeField] float mech_bottom_height = 2;

        [Header("Radius Settings")]
        [SerializeField] float mech_top_radius = 60;
        [SerializeField] float mech_middle_radius = 70;
        [SerializeField] float mech_bottom_radius = 30;


    void Awake(){
        PlayerEventPublisher.playerChangeToSubmarine += ChangeCameraToSubmarine;
        PlayerEventPublisher.playerChangeToSMech += ChangeCameraToMech;
        
    }


    void ChangeCameraToSubmarine(object source){
     
            cinemachineFreeLook.m_Orbits[0].m_Height = sub_top_height;
            cinemachineFreeLook.m_Orbits[1].m_Height = sub_middle_height;
            cinemachineFreeLook.m_Orbits[2].m_Height = sub_bottom_height;

            cinemachineFreeLook.m_Orbits[0].m_Radius = sub_top_radius;
            cinemachineFreeLook.m_Orbits[1].m_Radius = sub_middle_radius;
            cinemachineFreeLook.m_Orbits[2].m_Radius = sub_bottom_radius;
    }

    void ChangeCameraToMech(object source){
            cinemachineFreeLook.m_Orbits[0].m_Height = mech_top_height;
            cinemachineFreeLook.m_Orbits[1].m_Height = mech_middle_height;
            cinemachineFreeLook.m_Orbits[2].m_Height = mech_bottom_height;

            cinemachineFreeLook.m_Orbits[0].m_Radius = mech_top_radius;
            cinemachineFreeLook.m_Orbits[1].m_Radius = mech_middle_radius;
            cinemachineFreeLook.m_Orbits[2].m_Radius = mech_bottom_radius;
    }


     
}
