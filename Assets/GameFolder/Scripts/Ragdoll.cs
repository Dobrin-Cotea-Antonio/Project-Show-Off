using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    private void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        
        animator.enabled = true;
    }
    
    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidbodies)
        {
            rigidBody.isKinematic = false;
        }
        
        animator.enabled = false;
    }

    public void ApplyForce(Vector3 force)
    {
        var rigidBody = animator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rigidBody.AddForce(force, ForceMode.VelocityChange);
    }
}