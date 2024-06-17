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
        
        // player voice lines
        SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_CUTTING_MAST, 5f);
        
        // enemy voice lines
        //SoundManager.PlaySoundRepeating(SoundManager.Sound.VoiceLine_ENEMY_CUTTING_MAST, Random.Range(5,10), transform);
    }

    public override void OnStateExit() {
        weaponGameObject.SetActive(true);
        
        SoundManager.StopSound(SoundManager.Sound.SawingMast);
        
        // stop player and enemy voice lines
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_CUTTING_MAST);
        SoundManager.StopSound(SoundManager.Sound.VoiceLine_ENEMY_CUTTING_MAST);
        
    }
    #endregion
}