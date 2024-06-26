using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PistolScript : MonoBehaviour, IAttachable
{
    [Header("Pistol Data")] [SerializeField]
    Transform shootPoint;

    XRGrabInteractable interactableComponent;

    [Header("Shooting Data")] [SerializeField]
    GameObject bulletPrefab;

    [SerializeField] [Range(0, 1)] float cameraShakeIntensity;
    [SerializeField] float cameraShakeDuration;
    [SerializeField] [Range(0, 1)] float fovChangeIntensity;
    [SerializeField] float fovChangeDuration;
    [SerializeField] bool useStaticIntensity = true;
    [SerializeField] bool hasInfiniteBullets = false;
    [SerializeField] float shootCooldown;

    [Header("Bullet Data")] [SerializeField]
    float bulletSpeed;

    [SerializeField] float bulletDamage;
    [SerializeField] Vector3 bulletScale;

    float lastShotTime = -10000000;

    [Header("Static Intensity Data")] [SerializeField] [Range(0, 1)]
    float shootHapticIntensity;

    [SerializeField] float shootHapticDuration;

    [Header("Dinamic Intensity Data")] [SerializeField]
    AnimationCurve dynamicHapticIntensity;

    [SerializeField] float dynamicTotalHapticDuration;
    [SerializeField] float timeBetweenHapticIntensityChange;

    [Header("Reloading")] [SerializeField] InputActionProperty[] reloadActions;
    [SerializeField] BulletIndicatorScript reloadScript;
    [SerializeField] [Range(0, 1)] float reloadFailSpeedMultiplier;
    [SerializeField] int maxBulletCount;
    int currentBulletCount;

    bool hasShot = false;
    bool canAtteptReload = true;

    [Header("Tool Belt")] [SerializeField] Vector3 attachRotation;
    ToolBelt toolbeltAttachedTo = null;

    [Header("Bullet Trajectory")] [SerializeField]
    LineRenderer lineRenderer;

    [SerializeField] float lineRange;

    [Header("Particle Effects")] [SerializeField]
    private GameObject smokeParticleEffect;

    private bool hasPickedUpPistol = false;

    #region Unity Events

    private void Awake()
    {
        currentBulletCount = maxBulletCount;

        interactableComponent = GetComponent<XRGrabInteractable>();
        interactableComponent.activated.AddListener(Shoot);
        interactableComponent.activated.AddListener(TriggerHapticResponse);

        interactableComponent.selectEntered.AddListener(OnSelect);
        interactableComponent.selectExited.AddListener(OnDeselect);
        interactableComponent.selectEntered.AddListener(EnableLineRenderer);

        interactableComponent.selectExited.AddListener(DisableLineRenderer);

        if (lineRenderer != null)
        {
            DisableLineRenderer(null);
            Application.onBeforeRender += UpdateLineRenderer;
        }

        interactableComponent.retainTransformParent = false;

        if (!hasInfiniteBullets)
            reloadScript.EnableReloadMode(false);

        if (reloadScript != null)
        {
            reloadScript.OnCorrectInteraction += SuccessfullReload;
            reloadScript.OnIncorrectInteraction += FailedReload;
            reloadScript.OnFinish += SuccessfullReload;
        }

        interactableComponent.selectEntered.AddListener(OnPistolPickup);
    }

    private void Update()
    {
        if (toolbeltAttachedTo == null && transform.parent != null)
            transform.localPosition = Vector3.zero;

        AtteptReload(null);
    }

    #endregion

    #region Shooting

    void Shoot(ActivateEventArgs pArgs)
    {
        if (currentBulletCount == 0)
            return;

        if (Time.time - lastShotTime < shootCooldown)
            return;

        // Shoot
        SoundManager.PlaySound(SoundManager.Sound.Shooting, transform);

        currentBulletCount = Mathf.Max(currentBulletCount - 1, 0);
        hasShot = true;

        GameObject bulletGameobject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bulletGameobject.transform.localScale = bulletScale;

        BulletProjectileScript bullet = bulletGameobject.GetComponent<BulletProjectileScript>();
        bullet.speed = bulletSpeed;
        bullet.damage = bulletDamage;

        // Particle effect
        GameObject smoke = Instantiate(smokeParticleEffect, shootPoint);
        Destroy(smoke, 5);

        if (hasInfiniteBullets)
            currentBulletCount = maxBulletCount;

        if (currentBulletCount == 0)
            reloadScript.EnableReloadMode(true);

        lastShotTime = Time.time;

        if (reloadScript != null)
            reloadScript.UpdatePercentage(((float)currentBulletCount) / maxBulletCount);
    }

    void TriggerHapticResponse(BaseInteractionEventArgs pArgs)
    {
        if (!hasShot)
            return;

        hasShot = false;

        if (pArgs.interactorObject is XRBaseControllerInteractor)
            TriggerHapticResponse(((XRBaseControllerInteractor)pArgs.interactorObject).xrController);
    }

    void TriggerHapticResponse(XRBaseController pController)
    {
        if (useStaticIntensity)
            pController.SendHapticImpulse(shootHapticIntensity, shootHapticDuration);
        else
            StartCoroutine(HapticResponseCoroutine(pController));
    }

    IEnumerator HapticResponseCoroutine(XRBaseController pController)
    {
        float time = 0;

        while (time < dynamicTotalHapticDuration)
        {
            time += timeBetweenHapticIntensityChange;
            float impulse = dynamicHapticIntensity.Evaluate(time);

            if (impulse > 1)
                impulse = 1;

            pController.SendHapticImpulse(impulse, timeBetweenHapticIntensityChange);
            yield return new WaitForSeconds(timeBetweenHapticIntensityChange);
        }
    }

    #endregion

    #region Reloading

    void AtteptReload(ActivateEventArgs pArgs)
    {
        if (reloadScript == null)
            return;

        bool isPresed = false;

        if (currentBulletCount != 0)
            return;

        if (!canAtteptReload)
            return;

        foreach (InputActionProperty p in reloadActions)
            if (p.action.IsPressed())
            {
                isPresed = true;
                break;
            }

        if (!isPresed)
            return;

        reloadScript.CheckIfLineIsInDeadzone();
    }

    void SuccessfullReload()
    {
        currentBulletCount = maxBulletCount;
        reloadScript.UpdatePercentage(((float)currentBulletCount) / maxBulletCount);
        reloadScript.EnableReloadMode(false);
        canAtteptReload = true;
    }

    void FailedReload()
    {
        reloadScript.UpdateSpeed(reloadFailSpeedMultiplier);
        canAtteptReload = false;
    }

    #endregion

    #region Attachable

    public void Attach(ToolBelt pBelt)
    {
        toolbeltAttachedTo = pBelt;
        interactableComponent.selectExited.AddListener(PlaceOnToolbelt);
    }

    public void Detach(ToolBelt pBelt)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.useGravity = true;
    }

    public ToolBelt AttachedToolbelt()
    {
        return toolbeltAttachedTo;
    }

    void PlaceOnToolbelt(SelectExitEventArgs pArgs)
    {
        if (toolbeltAttachedTo == null)
            return;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        transform.parent = toolbeltAttachedTo.transform;
        transform.eulerAngles = attachRotation;
        transform.localPosition = Vector3.zero;

        rb.useGravity = false;
    }

    void OnSelect(SelectEnterEventArgs pArgs)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    void OnDeselect(SelectExitEventArgs pArgs)
    {
        PlaceOnToolbelt(null);
    }

    #endregion

    #region Bullet Trajectory

    void EnableLineRenderer(SelectEnterEventArgs pArgs)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.enabled = true;
    }

    void DisableLineRenderer(SelectExitEventArgs pArgs)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.enabled = false;
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null)
            return;

        if (lineRenderer.enabled)
        {
            Vector3[] points = new Vector3[2];
            points[0] = shootPoint.position;
            points[1] = shootPoint.position + lineRange * shootPoint.forward;
            lineRenderer.SetPositions(points);
        }
    }

    #endregion

    private void OnPistolPickup(SelectEnterEventArgs arg0)
    {
        if (!hasPickedUpPistol)
        {
            hasPickedUpPistol = true;
            SoundManager.PlaySound(SoundManager.Sound.VoiceLine_PLAYER_PICKS_UP_WEAPON);

            Debug.Log("Pistol picked up for the first time!");
        }
    }
}