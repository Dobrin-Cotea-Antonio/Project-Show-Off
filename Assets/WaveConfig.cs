using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "ScriptableObjects/WaveConfig", order = 2)]
public class WaveConfig : ScriptableObject
{
    public EnemyConfig[] enemyConfigs;
    public int enemyCount;
    public float delayBeforeWave;
    public float delayBetweenSpawns;
}
