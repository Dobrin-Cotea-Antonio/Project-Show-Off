using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] Animator animator;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isCutting", true);
            animator.applyRootMotion = true;
        }
    }
}
