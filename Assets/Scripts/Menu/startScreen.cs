using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class startScreen : MonoBehaviour
{
     [SerializeField]
    Text t1; 
     [SerializeField]
    int levelIndex;
    // Start is called before the first frame update
 private bool mouse_over = false;
     void Update()
     {
         if (mouse_over)
         {
             Debug.Log("Mouse Over");
         }
     }
 
   void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
    }    
}
