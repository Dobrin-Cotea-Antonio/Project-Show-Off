using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMastState : EnemyState
{
    [Header("Attack Data")] [SerializeField]
    float attackDelay;

    [SerializeField] float attackDamage;

    [Header("Animation")] [SerializeField] Animator animator;
    [SerializeField] GameObject weaponGameObject;

    MastScript mast;
    float lastAttackTime = -100000000;

    #region Unity Events

    private void Awake()
    {
        mast = EnemyManager.instance.mastTransform.GetComponent<MastScript>();
    }

    #endregion

    #region State Handling

    public override void Handle()
    {
        if (Time.time - lastAttackTime <= attackDelay)
            return;

        mast.TakeDamage(attackDamage);

        // play sawing mast sound
        SoundManager.PlaySound(SoundManager.Sound.SawingMast, transform);

        // play player voice lines
        if (Random.Range(1, 100) > 10)
        {
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_CUTTING_MAST);
        }

        // play enemy voice lines
        if (Random.Range(1, 100) > 20)
        {
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_ENEMY_CUTTING_MAST, transform);
        }

        lastAttackTime = Time.time;
    }

    public override void OnStateEnter()
    {
        weaponGameObject.SetActive(false);
    }

    public override void OnStateExit()
    {
        weaponGameObject.SetActive(true);
    }

    #endregion
}