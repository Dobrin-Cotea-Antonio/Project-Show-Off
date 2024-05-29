using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb : MonoBehaviour, IDamagable {
    [SerializeField] EnemyAI target;
    [SerializeField] [Range(0, 5)] float damageMultiplier;

    public void TakeDamage(float pDamage) {
        target.TakeDamage(pDamage * damageMultiplier);
    }
}
