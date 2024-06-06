using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour {

    public System.Action OnSuccessfulShot;

    [Header("Data")]
    [SerializeField] protected EnemyWeaponData data;

    protected float lastTimeShot = 0;

    #region Shooting
    public abstract void Shoot(Vector3 pTarget);
    #endregion
}
