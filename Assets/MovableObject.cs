using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveDuration = 2f;

    private void Start()
    {
       EventManager.instance.OnMoveObject.AddListener(() => StartCoroutine(MoveToTarget()));
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target.position;
        
        Quaternion startRotation = transform.rotation;
        Vector3 direction = (targetPosition - startPosition).normalized;
        Quaternion endRotattion = Quaternion.LookRotation(direction) * Quaternion.Euler(0,90,0);
        

        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / moveDuration);
            transform.rotation = Quaternion.Slerp(startRotation, endRotattion, timeElapsed / moveDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = endRotattion;
    }
}
