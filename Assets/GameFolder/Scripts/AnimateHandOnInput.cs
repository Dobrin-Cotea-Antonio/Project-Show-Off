using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour {
    [SerializeField] InputActionProperty pinchAnimationAction;
    [SerializeField] InputActionProperty gripAnimationAction;
    [SerializeField] Animator animator;

    private void Update() {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>();
        float gripValue= gripAnimationAction.action.ReadValue<float>();
        animator.SetFloat("Trigger",triggerValue);
        animator.SetFloat("Grip", gripValue);
    }
}
