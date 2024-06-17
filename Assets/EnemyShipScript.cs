using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.AI.Navigation;

public class EnemyShipScript : MonoBehaviour {

    public Action OnTargetReached;
    public Action OnBoardFinish;

    [Header("Movement")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] AnimationCurve shipSpeed;

    [Header("NavMesh Baking")]
    [SerializeField] NavMeshSurface navMesh;

    [Header("Plank")]
    [SerializeField] Transform plankPivot;
    [SerializeField] float targetXRotation;
    [SerializeField] float rotationSpeed;

    bool canMove = false;
    float epsilon = 0.1f;

    #region Unity Events
    private void Start() {
        OnTargetReached += StartRotatingPlank;
    }

    private void Update() {
        MoveShip();
    }
    #endregion

    #region Ship Movement
    private void MoveShip() {
        if (!canMove)
            return;

        if ((transform.position - endPoint.position).magnitude <= epsilon)
            return;

        float percentageComplete = Mathf.Abs((transform.position - startPoint.position).magnitude / (endPoint.position - startPoint.position).magnitude);

        Vector3 moveDirection = (endPoint.position - transform.position).normalized;
        float distance = (endPoint.position - transform.position).magnitude;
        float speed = Mathf.Clamp(shipSpeed.Evaluate(percentageComplete) * Time.deltaTime, 0, distance);

        transform.Translate(moveDirection * speed);

        if ((transform.position - endPoint.position).magnitude <= epsilon)
            OnTargetReached?.Invoke();
    }

    public void EnableMovement(bool pState) {
        canMove = pState;
    }
    #endregion

    #region Plank
    private void StartRotatingPlank() {
        StartCoroutine(RotatePlank());
    }

    private IEnumerator RotatePlank() {
        float remainingValue = targetXRotation - plankPivot.transform.localEulerAngles.x;
        float sign = Mathf.Sign(remainingValue);

        while (remainingValue != 0) {

            float value = Mathf.Min(rotationSpeed * Time.deltaTime, Mathf.Abs(remainingValue));
            remainingValue -= sign * value;
            plankPivot.localEulerAngles = new Vector3(plankPivot.localEulerAngles.x + sign * value, plankPivot.localEulerAngles.y, plankPivot.localEulerAngles.z);
            yield return 0;
        }

        OnBoardFinish?.Invoke();
    }
    #endregion
}
