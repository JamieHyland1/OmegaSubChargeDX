using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private GameObject SwordHitVFX;
    // [SerializeField]private GameObject MeleeHitVFX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider " + other.gameObject.name);
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<HitStop>().confirmHit();
            var collisionPoint = other.ClosestPoint(transform.position);
            GameObject obj = Instantiate(SwordHitVFX, collisionPoint, Quaternion.identity);
            obj.GetComponent<ParticleSystem>().Play();

        }
    }
}
