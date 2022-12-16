using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Omega Sub Charge/Enemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    public Mesh enemyModel;

    [SerializeField]
    public Material texture;

    [SerializeField]
    float speed;




}
