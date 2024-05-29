using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AIAgentConfig", menuName = "AI Stuff/AIAgentConfig")]
public class AIAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    public float dieForce = 10f;
    public float maxSightDistance = 5.0f;
    public float destroyTime = 10.0f;
    public float stoppingDistance = 1.0f;
}
