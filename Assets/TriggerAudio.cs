using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    public SoundManager.Sound sound;
    public bool is3Dsound = true;
    public float cooldownTime = 2f;
    private float nextTriggerTime;
    
    void Start()
    {
        nextTriggerTime = Random.value * 6;
    }
    
    void Update()
    {
            if (Time.time > nextTriggerTime)
            {
                if (is3Dsound)
                    SoundManager.PlaySound(sound, transform);
                else
                    SoundManager.PlaySound(sound);
                
                nextTriggerTime = Time.time + cooldownTime + Random.value * 3;
            }

    }
}
