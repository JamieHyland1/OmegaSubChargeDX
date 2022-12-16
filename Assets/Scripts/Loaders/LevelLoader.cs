using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;
    public static float transitionTime = .15f;
    // Update is called once per frame
   
   public int levelIndex = 0;



    
    public void LoadLevel(){
        Debug.Log("Loading");
       StartCoroutine(loadLevel(levelIndex));
    }

   


    public IEnumerator loadLevel(int levelIndex){
        yield return new WaitForSeconds(1f);
           // transition.SetTrigger("Play");
            yield return new WaitForSeconds(transitionTime);
           // transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(levelIndex);
    }

}