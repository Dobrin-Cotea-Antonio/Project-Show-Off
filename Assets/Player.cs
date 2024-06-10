using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

public class Player : MonoBehaviour, IDamagable {
    public Action<float, float> OnDamageTaken;
    public Action OnDeath;

    [Header("HP Data")]
    [SerializeField] float maxHp;
    public float hp { get; private set; }

    [Header("Arms")]
    [SerializeField] XRDirectInteractor[] _armInteractors;

    ContinuousMoveProviderBase moveProvider;

    [Tooltip("Left arm must be placed first")] public XRDirectInteractor[] armInteractors { get { return _armInteractors; } }
    public Vector3[] armMovementThisFrame { get; private set; }
    public int armsInUse { get; set; }

    private Vector3[] armPositionLastFrame;
    private Vector3[] armPositionThisFrame;

    #region Unity Events
    private void Awake() {
        hp = maxHp;
    }

    private void Start() {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
    }

    private void Update() {
        UpdateGravity();
        UpdateArms();
    }
    #endregion

    #region Arms 
    private void UpdateArms() {
        for (int i = 0; i < armInteractors.Length; i++) {
            armPositionLastFrame[i] = armPositionThisFrame[i];
            armPositionThisFrame[i] = armInteractors[i].transform.position;
            armMovementThisFrame[i] = armPositionThisFrame[i] - armPositionLastFrame[i];
        }

        Debug.Log(armMovementThisFrame[0]);
    }

    private void UpdateGravity() {
        if (armsInUse == 0)
            moveProvider.useGravity = true;
        else
            moveProvider.useGravity = false;
    }
    #endregion

    #region Damage
    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);

        if (hp == 0)
            OnDeath?.Invoke();
    }
    #endregion

    //#region Interactable Hover;
    //private void EnableHighlight(HoverEnterEventArgs pArgs) {
    //    Outline outline = pArgs.interactableObject.transform.GetComponent<Outline>();

    //    if (outline != null)
    //        outline.enabled = true;
    //}

    //private void DisableHighlight(HoverExitEventArgs pArgs) {
    //    Outline outline = pArgs.interactableObject.transform.GetComponent<Outline>();

    //    if (outline != null)
    //        outline.enabled = false;
    //}
    //#endregion
}
