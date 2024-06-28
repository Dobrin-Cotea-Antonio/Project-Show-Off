using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMastState : EnemyState
{
    [Header("Attack Data")] [SerializeField]
    float attackDamage;

    [Header("Animation")] [SerializeField] Animator animator;
    [SerializeField] GameObject weaponGameObject;
    [SerializeField] GameObject axeGameObject;

    MastScript mast;

    #region Unity Events

    private void Awake()
    {
        mast = EnemyManager.instance.mastTransform.GetComponent<MastScript>();

        if (axeGameObject != null)
            axeGameObject.SetActive(false);
    }

    [Header("Voice Lines")] [SerializeField]
    float voiceLineInterval = 5f;

    float lastVoiceLineTime;

    #endregion

    #region State Handling

    public override void Handle()
    {
        LookAtMast();
    }

    public void DamageMast()
    {
        PlayVoiceLines();
        mast.TakeDamage(attackDamage);
    }

    public override void OnStateEnter()
    {
        Debug.Log("Entering AttackMastState: Setting IsSawing to true");
        weaponGameObject.SetActive(false);

        axeGameObject.SetActive(true);

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
        Debug.Log("Exiting AttackMastState: Setting IsSawing to false");
        animator.SetBool("IsSawing", false);
        axeGameObject.SetActive(false);
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
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_PLAYER_ENEMY_CUTTING_MAST);
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_ENEMY_CUTTING_MAST, transform);
        
    }

    #endregion
}