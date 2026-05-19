using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class CarSFX : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource tireSlideSource;
    public AudioSource carEngineSource;
    public AudioSource carCrashSource;
    //Local var
    float desiredEnginePitch = 0.7f;
    float tireSlidePitch = 0.5f;
    TopControl topControl;
    void Awake()
    {
        topControl = GetComponent<TopControl>();
    }
    void Update()
    {
        UpdateEngineSFX();
        UpdateTireSlideSFX();
    }
    void UpdateEngineSFX()
    {
        float velocityMagnitude = topControl.GetVelocityMagnitude();
        float desiredEngineVolume = velocityMagnitude * 0.02f;
        desiredEngineVolume = Mathf.Clamp(desiredEngineVolume, 0.1f, 0.8f);
        carEngineSource.volume = Mathf.Lerp(carEngineSource.volume, desiredEngineVolume, Time.deltaTime * 10);
        desiredEnginePitch = velocityMagnitude * 0.2f;
        desiredEnginePitch = Mathf.Clamp(desiredEnginePitch, 0.7f, 2.5f);
        carEngineSource.pitch = Mathf.Lerp(carEngineSource.pitch, desiredEnginePitch, Time.deltaTime * 1.5f);

    }
    void UpdateTireSlideSFX()
    {
        if (topControl.IsTireSliding(out float lateralVelocity, out bool isBrakingNow))
        {
            if (isBrakingNow)
            {
                tireSlideSource.volume = Mathf.Lerp(tireSlideSource.volume, 0.7f, Time.deltaTime * 5);
                tireSlidePitch = Mathf.Lerp(tireSlidePitch, 0.5f, Time.deltaTime * 10);
            }
            else
            {
                tireSlideSource.volume = Mathf.Abs(lateralVelocity) * 0.05f;
                tireSlidePitch = Mathf.Abs(lateralVelocity) * 0.1f;
            }
        }
        else tireSlideSource.volume = Mathf.Lerp(tireSlideSource.volume, 0, Time.deltaTime * 10);
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        float relativeVelocity = collision2D.relativeVelocity.magnitude;
        float volume = relativeVelocity * 0.4f;
        carCrashSource.pitch = Random.Range(0.9f, 1.0f);
        carCrashSource.volume = volume;
        if (!carCrashSource.isPlaying)
        {
            carCrashSource.Play();
        }
    }
}