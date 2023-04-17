using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeToFade = .55f;
    private Material[] _materials;
    private float counter;
    
    void Start()
    {
        MeshRenderer[] m = this.GetComponentsInChildren<MeshRenderer>();
        _materials = m[0].materials;
        counter = timeToFade;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_Opacity",counter/timeToFade);
        }

       
        if(counter <= 0)
        {
            counter = timeToFade;
            // for (int i = 0; i < _materials.Length; i++)
            // {
            //     _materials[i].SetFloat("_Opacity",1);
            // }
            Debug.Log("Setting to false");
            this.gameObject.SetActive(false);
        }
    }
}
