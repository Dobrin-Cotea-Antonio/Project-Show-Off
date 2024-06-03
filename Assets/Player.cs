using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour, IDamagable {
    public Action<float, float> OnDamageTaken;
    public Action OnDeath;

    [Header("HP Data")]
    [SerializeField] float maxHp;
    public float hp { get; private set; }

    private void Awake() {
        hp = maxHp;
    }

    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);
        if (hp == 0)
            OnDeath?.Invoke();
    }
}
