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
    [SerializeField] GameObject bulletTriggerColliderGameObject;
    [SerializeField] GameObject rodTriggerColliderGameObject;

    GunState state = GunState.Loaded;
    bool hasShot = false;
    CameraEffects player;

    [Header("Pistol Rod")]
    [SerializeField] GameObject ownedRod;
    [SerializeField] int pistolLayer;
    [SerializeField] int rodLayer;

    Vector3 defaultRodPosition;
    Quaternion defaultRodRotation;

    [Header("Tool Belt")]
    [SerializeField] Vector3 attachRotation;
    ToolBelt toolbeltAttachedTo = null;
    //Transform lastHandInteractedWith = null;

    [Header("Bullet Trajectory")]
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float lineRange;

    #region Unity Events
    private void Awake() {
        interactableComponent = GetComponent<XRGrabInteractable>();
        interactableComponent.activated.AddListener(Shoot);
        interactableComponent.activated.AddListener(TriggerHapticResponse);
        bulletTriggerColliderGameObject.SetActive(false);
        rodTriggerColliderGameObject.SetActive(true);

        defaultRodPosition = ownedRod.transform.localPosition;
        defaultRodRotation = ownedRod.transform.localRotation;

        ownedRod.GetComponent<XRGrabInteractable>().selectExited.AddListener(PutBackRod);
        ownedRod.GetComponent<XRGrabInteractable>().selectEntered.AddListener(GrabRod);

        player = FindObjectOfType<CameraEffects>();

        interactableComponent.selectEntered.AddListener(OnSelect);
        interactableComponent.selectExited.AddListener(OnDeselect);
        interactableComponent.selectEntered.AddListener(EnableLineRenderer);

        interactableComponent.selectExited.AddListener(DisableLineRenderer);
        DisableLineRenderer(null);

        interactableComponent.retainTransformParent = false;
        Application.onBeforeRender += UpdateLineRenderer;
    }

    private void Update() {
        if (toolbeltAttachedTo == null && transform.parent != null)
            transform.localPosition = Vector3.zero;
    }
    #endregion

    #region Shooting 
    void Shoot(ActivateEventArgs pArgs) {
        if (state != GunState.Loaded)
            return;
        //play sound
        GameObject bulletGameobject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        if (!hasInfiniteBullets)
            state = GunState.Empty;

        hasShot = true;
        bulletTriggerColliderGameObject.SetActive(true);
        player.StartShake(cameraShakeIntensity, cameraShakeDuration);
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
    public void PutBulletIn() {
        if (state != GunState.Empty)
            return;

        state = GunState.BulletIn;
        bulletTriggerColliderGameObject.SetActive(false);
        rodTriggerColliderGameObject.SetActive(true);
        //play sound
    }

    public void Reload() {
        if (state != GunState.BulletIn)
            return;

        state = GunState.Loaded;
        bulletTriggerColliderGameObject.SetActive(false);
        rodTriggerColliderGameObject.SetActive(false);
    }
    #endregion

    #region Rod
    void GrabRod(BaseInteractionEventArgs pArgs) {
        ownedRod.transform.SetParent(null);
        ownedRod.GetComponent<Rigidbody>().isKinematic = false;
        Physics.IgnoreLayerCollision(rodLayer, pistolLayer, false);
    }

    void PutBackRod(BaseInteractionEventArgs pArgs) {
        ownedRod.transform.SetParent(transform);
        ownedRod.GetComponent<Rigidbody>().isKinematic = true;
        ownedRod.transform.localPosition = defaultRodPosition;
        ownedRod.transform.localRotation = defaultRodRotation;
        Physics.IgnoreLayerCollision(rodLayer, pistolLayer, true);
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
        //transform.parent = lastHandInteractedWith;
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
        //transform.localRotation = Quaternion.identity;
        rb.constraints = RigidbodyConstraints.None;
        //lastHandInteractedWith = pArgs.interactorObject.transform;

       // if (toolbeltAttachedTo == null)
       //    transform.parent = pArgs.interactorObject.transform;
    }

    void OnDeselect(SelectExitEventArgs pArgs) {
        if (toolbeltAttachedTo == null)
            transform.parent = null;

        //lastHandInteractedWith = null;
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
