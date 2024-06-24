using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CannonController : MonoBehaviour
{
    [SerializeField] GameObject cannonBallPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireForce = 1000f;
    [SerializeField] private float shootingCooldown = 3f;

    [Header("Particles")] 
    [SerializeField] private GameObject smokeParticles;
    [SerializeField] private Transform smokePoint;
    [SerializeField] private GameObject fireParticles;


    private XRSimpleInteractable interactor;

    private void Awake()
    {
        interactor = GetComponent<XRSimpleInteractable>();
        interactor.selectEntered.AddListener(Fire);
    }

    private void Fire(SelectEnterEventArgs interactor)
    {
        GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * fireForce);
        
        Destroy(cannonBall, 5f);

        // particles
        //instanciate the particls
        if (smokeParticles != null && smokePoint != null)
        {
            GameObject particle = Instantiate(smokeParticles, smokePoint.position, smokePoint.rotation);
            Destroy(particle, 2f);
        }

        if (fireParticles != null)
        {
            GameObject fire = Instantiate(fireParticles, firePoint.position, firePoint.rotation);
            Destroy(fire, 2f);
        }

        // sound
        SoundManager.PlaySound(SoundManager.Sound.CannonFire, firePoint);
    }

    private void OnDestroy()
    {
        interactor.selectEntered.RemoveListener(Fire);
    }
}