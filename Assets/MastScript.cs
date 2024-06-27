using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class MastScript : MonoBehaviour, IDamagable {
    public Action<float, float> OnDamageTaken;
    public Action OnDestroy;

    [Header("Hp Data")]
    [SerializeField] float maxHp;

    [Header("Mast")]
    [SerializeField] Transform mastTransform;

    public float hp { get; private set; }

    #region Unity Events
    private void Awake() {
        hp = maxHp;
        OnDestroy += DropMast;
        OnDamageTaken?.Invoke(hp, maxHp);
    }
    #endregion

    #region Damage
    public void TakeDamage(float pDamage) {
        if (hp == 0)
            return;

        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);

        if (hp == 0)
            OnDestroy?.Invoke();
    }

    void DropMast() {
        if (mastTransform == null)
            return;

        mastTransform.DORotate(new Vector3(90, 0, 0), 7.5f, RotateMode.Fast);
    }
    #endregion
}
