using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

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
    
    public UnityEvent OnMoveObject = new UnityEvent();
    

    public void StartShipInvasionLMAO(float time)
    {
        StartCoroutine(TriggerEventAfterTime(OnMoveObject, time));
    }
    
    public IEnumerator TriggerEventAfterTime(UnityEvent timedEvent, float time)
    {
        yield return new WaitForSeconds(time);
        timedEvent.Invoke();
    }
}


