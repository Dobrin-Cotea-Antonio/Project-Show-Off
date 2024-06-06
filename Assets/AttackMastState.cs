using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMastState : EnemyState {

    [Header("Attack Data")]
    [SerializeField] float attackDelay;
    [SerializeField] float attackDamage;

    [Header("Animation")]
    [SerializeField] Animator animator;
    [SerializeField] GameObject weaponGameObject;

    MastScript mast;
    float lastAttackTime = -100000000;

    #region Unity Events
    private void Awake() {
        mast = EnemyManager.instance.mastTransform.GetComponent<MastScript>();
    }
    #endregion

    #region State Handling
    public override void Handle() {
        if (Time.time - lastAttackTime <= attackDelay)
            return;

        mast.TakeDamage(attackDamage);
        lastAttackTime = Time.time;
    }

    public override void OnStateEnter() {
        weaponGameObject.SetActive(false);
        
        // Sound 
        SoundManager.PlaySound(SoundManager.Sound.SawingMast, transform);
    }

    public override void OnStateExit() {
        weaponGameObject.SetActive(true);
        
        SoundManager.StopSound(SoundManager.Sound.SawingMast);
        
    }
    #endregion
}