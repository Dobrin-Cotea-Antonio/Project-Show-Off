using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolScript : MonoBehaviour, IAttachable {
    public enum GunState {
        Loaded,
        Empty,
        BulletIn,
    }

    [Header("Pistol Data")]
    [SerializeField] Transform shootPoint;

    XRGrabInteractable interactableComponent;

    [Header("Shooting Data")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] [Range(0, 1)] float cameraShakeIntensity;
    [SerializeField] float cameraShakeDuration;
    [SerializeField] [Range(0, 1)] float fovChangeIntensity;
    [SerializeField] float fovChangeDuration;
    [SerializeField] bool useStaticIntensity = true;
    [SerializeField] bool hasInfiniteBullets = false;

    [Header("Static Intensity Data")]
    [SerializeField] [Range(0, 1)] float shootHapticIntensity;
    [SerializeField] float shootHapticDuration;

    [Header("Dinamic Intensity Data")]
    [SerializeField] AnimationCurve dynamicHapticIntensity;
    [SerializeField] float dynamicTotalHapticDuration;
    [SerializeField] float timeBetweenHapticIntensityChange;

    [Header("Gun State")]
    [SerializeField] BulletIndicatorScript reloadScript;
    [SerializeField] int maxBulletCount;
    int currentBulletCount;

    bool hasShot = false;
    CameraEffects player;

    [Header("Tool Belt")]
    [SerializeField] Vector3 attachRotation;
    ToolBelt toolbeltAttachedTo = null;

    [Header("Bullet Trajectory")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float lineRange;

    #region Unity Events
    private void Awake() {
        currentBulletCount = maxBulletCount;

        interactableComponent = GetComponent<XRGrabInteractable>();
        interactableComponent.activated.AddListener(Shoot);
        interactableComponent.activated.AddListener(TriggerHapticResponse);
        interactableComponent.activated.AddListener(AtteptReload);

        player = FindObjectOfType<CameraEffects>();

        interactableComponent.selectEntered.AddListener(OnSelect);
        interactableComponent.selectExited.AddListener(OnDeselect);
        interactableComponent.selectEntered.AddListener(EnableLineRenderer);

        interactableComponent.selectExited.AddListener(DisableLineRenderer);
        DisableLineRenderer(null);

        interactableComponent.retainTransformParent = false;
        Application.onBeforeRender += UpdateLineRenderer;

        reloadScript.EnableReloadMode(false);
        reloadScript.OnCorrectInteraction += SuccessfullReload;
        reloadScript.OnIncorrectInteraction += FailedReload;
    }

    private void Update() {
        if (toolbeltAttachedTo == null && transform.parent != null)
            transform.localPosition = Vector3.zero;
    }
    #endregion

    #region Shooting 
    void Shoot(ActivateEventArgs pArgs) {
        if (currentBulletCount == 0)
            return;

        currentBulletCount = Mathf.Max(currentBulletCount - 1, 0);
        hasShot = true;

        GameObject bulletGameobject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        if (hasInfiniteBullets)
            currentBulletCount = maxBulletCount;

        if (currentBulletCount == 0)
            reloadScript.EnableReloadMode(true);

        reloadScript.UpdatePercentage(((float)currentBulletCount) / maxBulletCount);

        //player.StartShake(cameraShakeIntensity, cameraShakeDuration);
        //player.StartFovChange(fovChangeIntensity, fovChangeDuration);
    }

    void TriggerHapticResponse(BaseInteractionEventArgs pArgs) {
        if (!hasShot)
            return;

        hasShot = false;

        if (pArgs.interactorObject is XRBaseControllerInteractor)
            TriggerHapticResponse(((XRBaseControllerInteractor)pArgs.interactorObject).xrController);
    }

    void TriggerHapticResponse(XRBaseController pController) {
        if (useStaticIntensity)
            pController.SendHapticImpulse(shootHapticIntensity, shootHapticDuration);
        else
            StartCoroutine(HapticResponseCoroutine(pController));
    }

    IEnumerator HapticResponseCoroutine(XRBaseController pController) {
        float time = 0;

        while (time < dynamicTotalHapticDuration) {
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
    void AtteptReload(ActivateEventArgs pArgs) {
        if (currentBulletCount != 0)
            return;

        reloadScript.CheckIfLineIsInDeadzone();
    }

    void SuccessfullReload() {
        currentBulletCount = maxBulletCount;
        reloadScript.UpdatePercentage(((float)currentBulletCount) / maxBulletCount);
        reloadScript.EnableReloadMode(false);
    }

    void FailedReload() {
        reloadScript.SelectRandomIndicatorLocation(false);
        Debug.Log("failed");
    }
    #endregion

    #region Attachable
    public void Attach(ToolBelt pBelt) {
        toolbeltAttachedTo = pBelt;
        interactableComponent.selectExited.AddListener(PlaceOnToolbelt);
    }

    public void Detach(ToolBelt pBelt) {
        Rigidbody rb = GetComponent<Rigidbody>();
        interactableComponent.selectExited.RemoveListener(PlaceOnToolbelt);

        toolbeltAttachedTo = null;
        transform.parent = null;

        rb.useGravity = true;
    }

    public ToolBelt AttachedToolbelt() {
        return toolbeltAttachedTo;
    }

    void PlaceOnToolbelt(SelectExitEventArgs pArgs) {
        if (toolbeltAttachedTo == null)
            return;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        transform.parent = toolbeltAttachedTo.transform;
        transform.eulerAngles = attachRotation;
        transform.localPosition = Vector3.zero;

        rb.useGravity = false;
    }

    void OnSelect(SelectEnterEventArgs pArgs) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
    }

    void OnDeselect(SelectExitEventArgs pArgs) {
        if (toolbeltAttachedTo == null)
            transform.parent = null;
    }
    #endregion

    #region Bullet Trajectory
    void EnableLineRenderer(SelectEnterEventArgs pArgs) {
        lineRenderer.enabled = true;
    }

    void DisableLineRenderer(SelectExitEventArgs pArgs) {
        lineRenderer.enabled = false;
    }

    private void UpdateLineRenderer() {
        if (lineRenderer.enabled) {
            Vector3[] points = new Vector3[2];
            points[0] = transform.position;
            points[1] = transform.position + lineRange * transform.forward;
            lineRenderer.SetPositions(points);
        }
    }
    #endregion
}
