using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ParticleSystemJobs;

public class Partical_Effect_Accelerator : MonoBehaviour
{
    [SerializeField]
    PlayerControls controls;
    public ParticleSystem bubbles;
    public ParticleSystem smoke;
    private float acceleration;
    private float boosting;
    public float bubbleStartRate;
    public float smokeStartRate;
    public float accelerateMultiplier;
    public float boostMultiplier;
   // public Animator propellerAnim;
    // public Player player;
    private AudioSource audio;
    
    private void Awake() {
        controls = new PlayerControls();
        audio = GetComponent<AudioSource>();

    }
    void OnEnable() {
        controls.Enable();
    }
    void OnDisable()   
    {
        controls.Disable();
    }
    

    


   
    void Update()
    {
         ParticleSystem.EmissionModule bubblesEm = bubbles.emission;
         ParticleSystem.EmissionModule smokeEm = smoke.emission;
         ParticleSystem.MainModule smokeMM = smoke.main;
         float _turbo = 1;
        
         acceleration = controls.WaterMove.Accelerate.ReadValue<float>();
         boosting = controls.WaterMove.Boost.ReadValue<float>();
         if (_turbo == 0) boosting = 0;
        
         bubblesEm.rateOverTime = bubbleStartRate + ((boosting * boostMultiplier) + (acceleration * accelerateMultiplier));
         smokeEm.rateOverTime = smokeStartRate + ((boosting * boostMultiplier) + (acceleration * accelerateMultiplier));
    
         smokeMM.startSize = 0.39f + (boosting*0.8f) + (acceleration*0.3f);
    
       //  propellerAnim.speed = 1 + (boosting*5) + (acceleration*3);
        
         // audio.pitch = 0.6f + (boosting*1.5f) + (acceleration*1.1f);
         // audio.volume = 0.6f + (boosting*0.3f) + (acceleration*0.2f);
        
        
        
    }
}
