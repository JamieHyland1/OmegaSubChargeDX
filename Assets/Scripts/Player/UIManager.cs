
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [SerializeField]
    public Slider boostSlider;
    [SerializeField]
    Text txt;
    public int anchorCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       //Debug.Log(boostSlider.value);
       txt.text = ""+anchorCount;
    }
}
