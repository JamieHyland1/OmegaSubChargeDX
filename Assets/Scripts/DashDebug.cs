
using UnityEngine;

public class DashDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position + transform.forward, transform.position + transform.forward*50,Color.black);
    }
}
