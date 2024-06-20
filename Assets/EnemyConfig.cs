using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig", order = 1)]
public class EnemyConfig : ScriptableObject
{
    public GameObject enemyPrefab;
    public int maxChopEnemiesAllowed;
}
