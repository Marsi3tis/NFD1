using UnityEngine;

public class TrailMarkRender : MonoBehaviour
{
    TopControl topControl;
    TrailRenderer trailRenderer;

    void Awake()
    {
        topControl = GetComponent<TopControl>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (topControl.IsTireSliding(out float lateralVelocity, out bool isBraking))
            trailRenderer.emitting = true;
        else trailRenderer.emitting = false;
    }
}
