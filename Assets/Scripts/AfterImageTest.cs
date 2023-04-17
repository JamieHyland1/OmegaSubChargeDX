using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AfterImageTest : MonoBehaviour
{
    [SerializeField]private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    
 
    void Start()
    {
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        for ( int j = 0; j < _skinnedMeshRenderers.Length; j++)
        {
             
            GameObject obj = new GameObject();
            obj.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            //obj.transform.Rotate(new Vector3(0,0,0));
            MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            _skinnedMeshRenderers[j].BakeMesh(mesh);
            meshFilter.mesh = mesh;
            meshRenderer.materials = _skinnedMeshRenderers[j].materials;
        }
    }
 
 
}
