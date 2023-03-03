using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    public float bobbleSpeed = 1f;
    public float bobbleAmount = 0.1f;
    public float noiseScale = 1f;

    private Vector3 basePosition;
    private float noiseOffset;

    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        // Generate a random noise offset
        noiseOffset += Time.deltaTime * bobbleSpeed;
        float noiseValue = Mathf.PerlinNoise(transform.position.x * noiseScale, transform.position.z * noiseScale + noiseOffset);

        // Calculate the bobble amount
        float bobble = (noiseValue - 0.5f) * 2f * bobbleAmount;

        // Apply the bobble effect to the object's position
        Vector3 position = basePosition + Vector3.up * bobble;
        transform.position = position;
    }
    }

