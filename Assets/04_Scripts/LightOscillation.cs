using UnityEngine;

[RequireComponent (typeof(Light))]
public class LightOscillation : MonoBehaviour
{
    public float intensityAmplitude = 1f;
    public float lightFrequency = 2f;

    private Light lightComponent;
    private float originalIntensity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightComponent = GetComponent<Light>();
        originalIntensity = lightComponent.intensity;
               
    }

    // Update is called once per frame
    void Update()
    {
        lightComponent.intensity = originalIntensity + Mathf.Sin(Time.time * lightFrequency) * intensityAmplitude;
    }
}
