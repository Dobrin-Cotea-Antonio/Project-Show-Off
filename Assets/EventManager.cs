using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent : UnityEvent
{
}

public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }

    // Create the timed events here
    public TimedEvent OnMoveBoatAfterTime = new TimedEvent();
    

    private void Start()
    {
        StartCoroutine(TriggerEventAfterTime(OnMoveBoatAfterTime, 5));
    }
    
    public IEnumerator TriggerEventAfterTime(TimedEvent timedEvent, float time)
    {
        yield return new WaitForSeconds(time);
        timedEvent.Invoke();
    }
}


