using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    public static EventManager instance;
    private void Awake() {
        if (!instance) {
            instance = this;
        }
    }
    public UnityEvent OnMoveObject = new UnityEvent();


    public void StartShipInvasion(float time) {
        StartCoroutine(TriggerEventAfterTime(OnMoveObject, time));
    }


    public IEnumerator TriggerEventAfterTime(UnityEvent timedEvent, float time) {
        yield return new WaitForSeconds(time);
        timedEvent.Invoke();
    }
}


