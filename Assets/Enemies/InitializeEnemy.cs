using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    EnemySO obj;
    MeshFilter mf;
    MeshRenderer mr;
    void Start()
    {
        mf = GetComponent<MeshFilter>();
        mf.mesh = obj.enemyModel;
        mr.materials[0] = obj.texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
