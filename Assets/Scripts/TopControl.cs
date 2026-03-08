using System.Numerics;
using Microsoft.VisualBasic;
using UnityEngine;
public class TopControl : MonoBehaviour
{
    [Header("Car Settings")]
    public float accelerationFactor = 20.0f;
    public float turnFactor = 2.0f;

    //Local variables
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    
    //Components
    Rigidbody2D carRigidbody2D;
    
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();

    }
    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
    }
    void ApplyEngineForce()
    {
        Vector2 engineForceVector = trasform.up * accelerationInput * accelerationFactor;

        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);

    }
    void ApplySteering()
    {
        rotationAngle -= steeringInput * turnFactor;

        carRigidbody2D.MoveRotation(rota)
    }
}
