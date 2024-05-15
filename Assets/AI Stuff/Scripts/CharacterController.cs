using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;

    private void OnValidate()
    {
        if(!agent) agent = GetComponent<NavMeshAgent>();
        if(!animator) animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (agent.hasPath)
        {
            var dir = (agent.steeringTarget - transform.position).normalized;
            var animDir = transform.InverseTransformDirection(dir);
            var isFacingMoveDir = Vector3.Dot(dir, transform.forward) > .5f;
            
            //animator.SetFloat("Hotizontal",isFacingMoveDir ? animDir.x : 0, .5f, Time.deltaTime);
            animator.SetFloat("Vertical",isFacingMoveDir ? animDir.z : 0, .5f, Time.deltaTime);
            
            transform.rotation = Quaternion.RotateTowards( transform.rotation, Quaternion.LookRotation(dir), 180 * Time.deltaTime);

            if (Vector3.Distance(transform.position, agent.destination) < agent.radius)
            {
                agent.ResetPath();
            }
        }
        else
        {
            //animator.SetFloat("Hotizontal", 0, .25f, Time.deltaTime);
            animator.SetFloat("Vertical", 0, .25f, Time.deltaTime);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var isHit = Physics.Raycast(ray, out var hit, 20);
            if (isHit)
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < agent.path.corners.Length -1; i++)
        {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i + 1], Color.red);
        }
    }
}
