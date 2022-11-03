using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;

    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void updateScore(){
        score++;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = ""+score;
    }
}
