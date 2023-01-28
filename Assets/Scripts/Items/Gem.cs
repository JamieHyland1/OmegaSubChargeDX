// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Gem : MonoBehaviour
// {
//     [SerializeField]
//     int GemHealth = 1000;
//     private AudioSource audio;
//     public AudioClip audioClip;

//     [SerializeField]
//     Text ui;

//     // Start is called before the first frame update
//     void Start()
//     {
//         audio = this.GetComponent<AudioSource>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         ui.text = ""+GemHealth;
//         if(GemHealth <= 0)Destroy(this);
//     }

//      void OnTriggerEnter(Collider other)
//     {
//         //Debug.Log(other.gameObject.name);
//         if(other.gameObject.tag == "Enemy"){
//            GemHealth--;
//            Destroy(other.gameObject);
//            audio.PlayOneShot(audioClip);
            
//         }
//     }
// }
