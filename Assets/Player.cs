using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class Player : MonoBehaviour, IDamagable {
    public Action<float, float> OnDamageTaken;
    public Action OnDeath;

    [Header("HP Data")]
    [SerializeField] float maxHp;
    public float hp { get; private set; }

    [Header("Arms")]
    [SerializeField] private List<XRDirectInteractor> _armInteractors;

    public List<XRDirectInteractor> armInteractors { get { return _armInteractors; } }
    public Vector3[] armMovementThisFrame { get; private set; }
    public Dictionary<XRDirectInteractor, bool> armUseStatus { get; private set; }
    public Dictionary<XRDirectInteractor, Vector3> armLockPosition { get; private set; }
    public int armsInUse { get; private set; }

    private Vector3[] armPositionLastFrame = new Vector3[2];
    private Vector3[] armPositionThisFrame = new Vector3[2];

    private ContinuousMoveProviderBase moveProvider;
    private ContinuousTurnProviderBase turnProvider;

    #region Unity Events
    private void Awake() {
        armMovementThisFrame = new Vector3[2];

        hp = maxHp;

        armLockPosition = new Dictionary<XRDirectInteractor, Vector3>();
        armLockPosition[armInteractors[0]] = Vector3.zero;
        armLockPosition[armInteractors[1]] = Vector3.zero;

        armUseStatus = new Dictionary<XRDirectInteractor, bool>();
        armUseStatus[armInteractors[0]] = false;
        armUseStatus[armInteractors[1]] = false;

        foreach (XRDirectInteractor interactor in armInteractors) {
            interactor.selectEntered.AddListener(MarkArmAsUsed);
            interactor.selectExited.AddListener(MarkArmAsUnused);
        }
    }

    private void Start() {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        turnProvider = GetComponent<ContinuousTurnProviderBase>();
    }

    private void Update() {
        UpdateGravity();
        UpdateArms();
        UpdateMovement();
    }

    private void LateUpdate() {
        UpdateArmLocation();
    }
    #endregion

    #region Arms
    private void UpdateArmLocation() {
        foreach (XRDirectInteractor interactor in armInteractors)
            if (armLockPosition[interactor] != Vector3.zero) {
                Debug.Log(armLockPosition[interactor]);
                interactor.transform.position = armLockPosition[interactor];
            }
    }

    private void UpdateArms() {
        for (int i = 0; i < armInteractors.Count; i++) {
            armPositionLastFrame[i] = armPositionThisFrame[i];
            armPositionThisFrame[i] = armInteractors[i].transform.localPosition;
            armMovementThisFrame[i] = armPositionThisFrame[i] - armPositionLastFrame[i];
        }
    }

    private void UpdateGravity() {
        if (armsInUse == 0)
            moveProvider.useGravity = true;
        else
            moveProvider.useGravity = false;
    }

    private void UpdateMovement() {
        if (armsInUse == 0) {
            moveProvider.enabled = true;
            turnProvider.enabled = true;
        } else {
            moveProvider.enabled = false;
            turnProvider.enabled = false;
        }
    }

    private void MarkArmAsUsed(SelectEnterEventArgs pArgs) {
        XRDirectInteractor interactor = pArgs.interactorObject.transform.GetComponent<XRDirectInteractor>();
        XRBaseInteractable interactable = pArgs.interactableObject.transform.GetComponent<XRBaseInteractable>();

        ClimbableObject climbableObject = interactable.GetComponent<ClimbableObject>();

        if (climbableObject == null)
            return;

        armsInUse++;

        int index = ReturnArmIndex(interactor);

        climbableObject.SetLastArmInteractedWith(interactor, index);
        climbableObject.SelectEnter(interactor, index);
        climbableObject.ChangeArmUseValue(true);

        armLockPosition[interactor] = interactor.transform.position;

        armUseStatus[interactor] = true;
    }

    private void MarkArmAsUnused(SelectExitEventArgs pArgs) {
        XRDirectInteractor interactor = pArgs.interactorObject.transform.GetComponent<XRDirectInteractor>();
        XRBaseInteractable interactable = pArgs.interactableObject.transform.GetComponent<XRBaseInteractable>();

        ClimbableObject climbableObject = interactable.GetComponent<ClimbableObject>();

        if (climbableObject == null)
            return;

        armsInUse--;

        int index = ReturnArmIndex(interactor);

        climbableObject.SelectExit(interactor, index);
        climbableObject.ChangeArmUseValue(false);

        armLockPosition[interactor] = Vector3.zero;

        armUseStatus[interactor] = false;
    }

    public int ReturnArmIndex(XRDirectInteractor pArm) {
        int index = -1;

        for (int i = 0; i < armInteractors.Count; i++)
            if (armInteractors[i] == pArm) {
                index = i;
                break;
            }

        return index;
    }

    //0 first arm 1 second arm -1 both are free or used
    public int ReturnFreeArmIndex() {
        if (armUseStatus[armInteractors[0]] && !armUseStatus[armInteractors[1]])
            return 0;

        if (armUseStatus[armInteractors[1]] && !armUseStatus[armInteractors[0]])
            return 1;

        return -1;
    }

    public Vector3 ReturnArmMiddlePoint() {
        return (armInteractors[0].transform.position + armInteractors[1].transform.position) / 2;
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
