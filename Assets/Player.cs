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

    private ContinuousMoveProviderBase moveProvider;
    private ContinuousTurnProviderBase turnProvider;

    #region Unity Events
    private void Awake() {
        hp = maxHp;
    }

    private void Start() {
        moveProvider = GetComponent<ContinuousMoveProviderBase>();
        turnProvider = GetComponent<ContinuousTurnProviderBase>();
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
}
