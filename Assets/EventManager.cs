using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    
    public TimedEvent OnMoveObject = new TimedEvent();
    

    public void StartShipInvasion(float time)
    {
        StartCoroutine(TriggerEventAfterTime(OnMoveObject, time));
    }
    
    public IEnumerator TriggerEventAfterTime(TimedEvent timedEvent, float time)
    {
        yield return new WaitForSeconds(time);
        timedEvent.Invoke();
    }
}


