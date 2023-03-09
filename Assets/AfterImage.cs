using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Vector3 = System.Numerics.Vector3;

public class AfterImage : MonoBehaviour
{
    [SerializeField]private SkinnedMeshRenderer[] _skinnedMeshRenderers;
    [SerializeField]private Material[] _dashMaterials;
    [SerializeField]private float timer = 0.25f;
    private List<GameObject> pooledObjects;
    [SerializeField]private int amountToPool;
    private float counter;
    private bool dashing = false;
    void Start()
    {
        counter = timer;
        PlayerEventPublisher.dashEvent += RecieveDashRequest;
        _skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = new GameObject();
            MeshRenderer meshRenderer = tmp.AddComponent<MeshRenderer>();
            MeshFilter meshFilter = tmp.AddComponent<MeshFilter>();
            tmp.AddComponent<Fade>();
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0 && dashing == true)
        {
            DisplayAfterImage();
            counter = timer;
            dashing = false;
        }
    }

    GameObject GetFromPool()
    {
        
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            GameObject obj = pooledObjects[i];
            if (!obj.active) return obj;
        }

        return null;
    }

    void RecieveDashRequest(object source)
    {
        dashing = true;
    }

    void DisplayAfterImage()
    {
        GameObject obj = GetFromPool();
        if (obj is null) return;
        for ( int i = 0; i < 1; i++)
        {
            obj.SetActive(true);
            obj.transform.SetPositionAndRotation(this.transform.position, this.transform.rotation);
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            _skinnedMeshRenderers[i].BakeMesh(mesh);
            meshFilter.mesh = mesh;
            meshRenderer.materials = _dashMaterials;
        }

    }

    private void OnDestroy()
    {
        PlayerEventPublisher.dashEvent -= RecieveDashRequest;
    }
}
