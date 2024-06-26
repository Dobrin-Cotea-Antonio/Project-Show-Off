using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMastState : EnemyState
{
    [Header("Attack Data")]
    [SerializeField] float attackDamage;

    [Header("Animation")] [SerializeField] Animator animator;
    [SerializeField] GameObject weaponGameObject;

    MastScript mast;

    #region Unity Events

    private void Awake()
    {
        mast = EnemyManager.instance.mastTransform.GetComponent<MastScript>();
    }

    [Header("Voice Lines")]
    [SerializeField] float voiceLineInterval = 5f;
    [SerializeField] float chanceOfEnemyVoiceLine = 10;
    [SerializeField] float chanceOfPlayerVoiceLine = 10;

    float lastVoiceLineTime;

    #endregion

    #region State Handling

    public override void Handle()
    {
        LookAtMast();

        if (Time.time - lastVoiceLineTime >= voiceLineInterval)
        {
            PlayVoiceLines();
            lastVoiceLineTime = Time.time;
        }

        lastVoiceLineTime = Time.time;
    }

    public void DamageMast()
    {
        mast.TakeDamage(attackDamage);
    }

    public override void OnStateEnter()
    {
        weaponGameObject.SetActive(false);



        if (animator != null)
        {
            animator.SetBool("IsSawing", true);
        }
        else
        {
            Debug.LogError("Animator is not assigned");
        }

        LookAtMast();
    }

    public override void OnStateExit()
    {
        weaponGameObject.SetActive(true);
        animator.SetBool("IsSawing", false);
    }

    #endregion

    #region Helper Methods

    private void LookAtMast()
    {
        if (mast != null)
        {
            Vector3 direction = (mast.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void PlayVoiceLines()
    {
        // play player voice lines
        if (Random.Range(1, 100) > chanceOfPlayerVoiceLine)
        {
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_CUTTING_MAST);
        }

        // play enemy voice lines
        if (Random.Range(1, 100) > chanceOfEnemyVoiceLine)
        {
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_ENEMY_CUTTING_MAST, transform);
        }
    }

    #endregion
}