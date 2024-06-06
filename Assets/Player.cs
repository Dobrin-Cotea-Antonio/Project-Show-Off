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
    [SerializeField] XRRayInteractor[] armInteractors;
    [SerializeField] float armRange;

    #region Unity Events
    private void Awake() {
        hp = maxHp;

        foreach (XRRayInteractor x in armInteractors) {
            x.hoverEntered.AddListener(EnableHighlight);
            x.hoverExited.AddListener(DisableHighlight);
            x.maxRaycastDistance = armRange;
        }
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

    #region Interactable Hover;
    private void EnableHighlight(HoverEnterEventArgs pArgs) {
        Outline outline = pArgs.interactableObject.transform.GetComponent<Outline>();

        if (outline != null)
            outline.enabled = true;
    }

    private void DisableHighlight(HoverExitEventArgs pArgs) {
        Outline outline = pArgs.interactableObject.transform.GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;
    }
    #endregion
}
