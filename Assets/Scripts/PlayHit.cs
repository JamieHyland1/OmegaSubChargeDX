
using UnityEngine;

public class PlayHit : MonoBehaviour
{
   
    [SerializeField]
    AudioSource audio;

    //Play the music
    bool m_Play;
    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;

    void Start()
    {
        //Fetch the AudioSource from the GameObject
      
        //Ensure the toggle is set to true for the music to play at start-up
        m_Play = true;
    }

    public void playHit()
    {
        if(audio.isPlaying)audio.Stop();
        audio.Play();
        // //Check to see if you just set the toggle to positive
        // if (m_Play == true && m_ToggleChange == true)
        // {
        //     //Play the audio you attach to the AudioSource component
        //     m_MyAudioSource.Play();
        //     //Ensure audio doesn’t play more than once
        //     m_ToggleChange = false;
        // }
        // //Check if you just set the toggle to false
        // if (m_Play == false && m_ToggleChange == true)
        // {
        //     //Stop the audio
        //     m_MyAudioSource.Stop();
        //     //Ensure audio doesn’t play more than once
        //     m_ToggleChange = false;
        // }
    }
}
