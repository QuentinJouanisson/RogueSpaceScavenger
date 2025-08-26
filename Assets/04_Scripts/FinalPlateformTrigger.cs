using UnityEngine;

public class FinalPlateformTrigger : MonoBehaviour
{
    [Header("MainLights to deactivate")]
    public Light mainLight;

    [Header("EnvMap settings")]
    public float newAmbientintensity = 0.2f;

    private float originalAmbientIntensity;    
    private Light[] childLights;
    

    private void Start()
    {
        
        childLights = GetComponentsInChildren<Light>(true);
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        if (mainLight != null)        
            mainLight.enabled =true;
        for(int i = 0; i < childLights.Length; i++)
        {
            childLights[i].enabled = false;
        }
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Final PF");
            if(mainLight != null)
                mainLight.enabled = false;
            Light[] childLights = GetComponentsInChildren<Light>(true);
            foreach (Light l in childLights)
            {
                if(l != mainLight)
                    l.enabled = true;
            }
            RenderSettings.ambientIntensity = newAmbientintensity;
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(mainLight != null)
                mainLight.enabled = true;
            for(int i =0; i < childLights.Length;i++)
                childLights[i].enabled = false;
            RenderSettings.ambientIntensity = originalAmbientIntensity;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
   
}
