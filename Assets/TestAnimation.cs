using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string animationName;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play(animationName);
        }
    }
}
