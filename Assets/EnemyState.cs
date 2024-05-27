using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EnemyStateID {
    Chase,
    FindCover,
    Shoot,
    ChaseAndShoot,
    TargetMast,
    DamageMast,
    Death,
}

public abstract class EnemyState : MonoBehaviour {

    [Header("State Data")]
    [SerializeField] protected EnemyAI owner;
    [SerializeField] protected EnemyStateID _stateID;
    public EnemyStateID stateID { get { return _stateID; } }

    #region State Handling
    public abstract void OnStateEnter();
    public abstract void OnStateExit();
    public abstract void Handle();
    #endregion
}
