using System.Collections;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    CinemachineFreeLook cmFreeCam;
    float counter = 0;

    void Awake()
    {
       // cmFreeCam = GetComponent<CinemachineFreeLook>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(counter > 0){
        //     counter -= Time.deltaTime;
        //     if(counter <= 0){
        //         for(int i = 0; i < 3; i ++){
        //             CinemachineVirtualCamera c = cmFreeCam.GetRig(i);
        //             CinemachineBasicMultiChannelPerlin noise = c.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //             noise.m_AmplitudeGain = 0;
        //             noise.m_m_FrequencyGain = 0;
        //         }
        //     }
        // }
    }

    public void Shake(){
        StartCoroutine(_ProcessShake(6,1));
    }

        private IEnumerator _ProcessShake(float shakeIntensity = 5f, float shakeTiming = 0.5f){    
            Noise(1, shakeIntensity);
            yield return new WaitForSeconds(shakeTiming);
            Noise(0, 0);
        }
 
        public void Noise(float amplitudeGain, float frequencyGain)
        {
            for(int i = 0; i < 3; i ++){
                // CinemachineVirtualCamera c = cmFreeCam.GetRig(i);
                // CinemachineBasicMultiChannelPerlin noise = c.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                // noise.m_AmplitudeGain = amplitudeGain;
                // noise.m_FrequencyGain = frequencyGain;

            }
        }
}
