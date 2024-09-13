using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    [Header("Random Rotation")]
    public float minSpeed = -15f;
    public float maxSpeed = 15f;

    [Header("Fixed Rotation")]
    public float fixedRotationSpeed = 30f;
    public bool useFixedRotation;

    private float rotationSpeed;

    private void Start()
    {
        if (useFixedRotation) {
            rotationSpeed = fixedRotationSpeed;
        }
        else
        {
            rotationSpeed = Random.Range(minSpeed, maxSpeed);
            if (rotationSpeed > -5 && rotationSpeed < 5)
            {
                rotationSpeed = 5;
            }
        }
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
