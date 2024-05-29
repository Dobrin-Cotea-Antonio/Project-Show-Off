using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyState {

    [Header("Death Data")]
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] float ragdollLifetime;

    #region Unity Events
    private void Awake() {
        owner.OnDeath += SwitchToDeathState;
    }
    #endregion

    #region State Handling
    public override void Handle() { }

    public override void OnStateEnter() {
        ragdoll.ActivateRagdoll();
        Destroy(ragdoll.gameObject, ragdollLifetime);
    }

    public override void OnStateExit() { }
    #endregion

    #region Helper Methods
    private void SwitchToDeathState(EnemyAI pEnemy) {
        if (pEnemy != owner)
            return;

        owner.SwitchState(stateID);
    }
    #endregion
}
