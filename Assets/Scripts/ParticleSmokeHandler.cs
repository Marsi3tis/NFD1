using System;
using UnityEngine;

public class ParticleSmokeHandler : MonoBehaviour
{
    float particleEmissionRate = 0;
    TopControl topControl;
    ParticleSystem particleSystemSmoke;
    ParticleSystem.EmissionModule particleSystemEmissionModule;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        topControl = GetComponentInParent<TopControl>();
        particleSystemSmoke = GetComponent<ParticleSystem>();
        particleSystemEmissionModule = particleSystemSmoke.emission;
        particleSystemEmissionModule.rateOverTime = 0;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 4);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if (topControl.IsTireSliding(out float lateralVelocity, out bool isBrakingNow))
        {
            if(isBrakingNow) particleEmissionRate = 30;
            else particleEmissionRate = MathF.Abs(lateralVelocity) * 2;
        }
    }
}
