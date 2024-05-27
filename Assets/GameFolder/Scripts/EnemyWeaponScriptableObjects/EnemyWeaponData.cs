using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Weapon", menuName = "Enemy Weapon")]
public class EnemyWeaponData : ScriptableObject {
    public float delayBetweenShots;
    [Range(0, 1)] public float chanceToShootAfterDelay;

    public float bulletSpeed;
    public float damage;
}
