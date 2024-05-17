using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerHealth : Health
{
    private VolumeProfile postProcessing;
    
    protected override void OnStart()
    {
        //postProcessing = FindObjectOfType<Volume>().profile;
    }

    protected override void OnDeath(Vector3 direction)
    {
        Debug.Log("Player died");
    }

    protected override void OnDamage(Vector3 direction)
    {
        /*Vignette vignette;
        if (postProcessing.TryGet(out vignette))
        {
            float percentage = 1 - (currentHealth / maxHealth);
            vignette.intensity.value = percentage * 0.5f;
        }*/
    }
}
