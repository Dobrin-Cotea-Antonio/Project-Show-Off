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

    [SerializeField] int waterLayer;

    private void Awake() {
        hp = maxHp;
    }

    public void TakeDamage(float pDamage) {
        hp = Mathf.Max(hp - pDamage, 0);
        OnDamageTaken?.Invoke(hp, maxHp);

        if (hp == 0)
            OnDeath?.Invoke();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.layer + " " + other.gameObject.name + " " + waterLayer);

        if (other.gameObject.layer == waterLayer) {
            Debug.Log("test");
            OnDeath?.Invoke();
        }

    }
}
