using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;
public class TopControl : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor = 0.1f; // normal grip
    public float accelerationFactor = 4f;
    public float deaccelerationFactor = 2.0f;
    public float turnFactor = 2.0f;
    public float maxSpeed = 10;
    public float reverseFactor = 0.5f;
     [Header("Brake Settings")]
    public float brakeForce = 3.0f;         
    public float handBrakeDriftFactor = 0.97f; // daugiau sonaslydzio
    public float switchDirectionThreshold = 0.5f;
    //Local variables
    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;
    bool isBraking = false;
    bool isHandBraking = false;
    //Components
    Rigidbody2D carRigidbody2D;
    
    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        ApplyEngineForce();
        KillVelocity();
        ApplySteering();
    }
    void ApplyEngineForce()
    {

        //apskaiciuoja kaip greitai vaziuoja masina
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.linearVelocity);
        
        // Jei spaudi W, bet dar vaziuoji atbulas -> stabdo
        if (accelerationInput > 0 && velocityVsUp < -switchDirectionThreshold)
        {
            carRigidbody2D.AddForce(transform.up * accelerationInput * accelerationFactor * brakeForce, ForceMode2D.Force);
            return;
        }

        // Jei spaudi S, bet dar vaziuoji pirmyn -> stabdo
        if (accelerationInput < 0 && velocityVsUp > switchDirectionThreshold)
        {
            carRigidbody2D.AddForce(transform.up * accelerationInput * accelerationFactor * brakeForce, ForceMode2D.Force);
            return;
        }


        //max speed, limituoja kaip greitai gali vaziuot
        if(velocityVsUp > maxSpeed && accelerationInput > 0)
            return;
        //max reverse greitis
        if(velocityVsUp < -maxSpeed * reverseFactor && accelerationInput < 0)
            return;
        if(carRigidbody2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;
    // drag, letina masinos greiti jei nelaikomas accelerationInput
        if(accelerationInput == 0)
        {
            carRigidbody2D.linearDamping = Mathf.Lerp(
            carRigidbody2D.linearDamping,
            deaccelerationFactor,
            Time.fixedDeltaTime * 3
        );
        }
        else
        {
            carRigidbody2D.linearDamping = 0;
        }
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);      


    }
    void ApplySteering()
    {
        float minSpeedBeforeTurining = (carRigidbody2D.linearVelocity.magnitude / 8);
        minSpeedBeforeTurining = Mathf.Clamp01(minSpeedBeforeTurining);
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeTurining;
        float currentTurnFactor = turnFactor;

        carRigidbody2D.MoveRotation(rotationAngle);
    }
    void KillVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);
        float currentDriftFactor = isHandBraking ? handBrakeDriftFactor : driftFactor;
        if(isHandBraking) currentDriftFactor *= handBrakeDriftFactor;
        currentDriftFactor = Mathf.Lerp(currentDriftFactor, driftFactor, Time.fixedDeltaTime * 3f);
        carRigidbody2D.linearVelocity = forwardVelocity + rightVelocity * currentDriftFactor;
        
    }
    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.linearVelocity);
    }    
    public void SetBrake(bool brakeInput)
    {
        isHandBraking = brakeInput;
    }
    public bool IsTireSliding(out float lateralVelocity, out bool isBrakingNow)
    {
        lateralVelocity = GetLateralVelocity();
        isBrakingNow = false;
        if(accelerationInput < 0 && velocityVsUp > 0)
        {
            isBraking = true;
            return true;
        }
        if(Mathf.Abs(GetLateralVelocity()) > 1.5f)
            return true;

        return false;
    }
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

}
