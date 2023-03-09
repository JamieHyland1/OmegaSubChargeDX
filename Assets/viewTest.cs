using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewTest : MonoBehaviour
{
    // Start is called before the first frame update
    public MeshRenderer Renderer;
    void Start()
    {
        this.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Visible " + Renderer.isVisible);
    }
}
