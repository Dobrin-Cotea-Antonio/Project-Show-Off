using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolScript : MonoBehaviour {
    public enum GunState {
        Loaded,
        Empty,
        BulletIn,
    }

    [Header("Pistol Data")]
    [SerializeField] Transform shootPoint;

    [Header("Shooting Data")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] bool useStaticIntensity = true;

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
    [Tooltip("Value is a percentage")] [SerializeField] int rodReloadChance;

    [Header("Pistol Rod")]
    [SerializeField] GameObject ownedRod;
    [SerializeField] int pistolLayer;
    [SerializeField] int rodLayer;

    Vector3 defaultRodPosition;
    Quaternion defaultRodRotation;

    GunState state = GunState.Loaded;
    bool hasShot = false;

    XRGrabInteractable interactableComponent;

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
    }

    #endregion

    #region Shooting 
    void Shoot(ActivateEventArgs pArgs) {
        if (state != GunState.Loaded)
            return;
        //play sound
        GameObject bulletGameobject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        state = GunState.Empty;
        hasShot = true;
        bulletTriggerColliderGameObject.SetActive(true);

    }

    void TriggerHapticResponse(BaseInteractionEventArgs pArgs) {
        if (!hasShot)
            return;

        hasShot = false;

        if (pArgs.interactorObject is XRBaseControllerInteractor)
            TriggerHapticResponse(((XRBaseControllerInteractor)pArgs.interactorObject).xrController);
    }

    void TriggerHapticResponse(XRBaseController pController) {
        Debug.Log("vibrating");

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

    //returns true if the reload was successfull otherwise return false
    public bool AtteptRodReload() {
        if (state != GunState.BulletIn)
            return false;

        int randomNumber = Random.Range(0, 100);
        if (randomNumber < rodReloadChance) {
            Debug.Log(randomNumber);
            state = GunState.Loaded;
            bulletTriggerColliderGameObject.SetActive(false);
            rodTriggerColliderGameObject.SetActive(false);
            //play sound to notify the player the reload was successful
            return true;
        }

        //play sound to notify the player the reload was unsuccessful
        return false;
    }
    #endregion

    #region Rod
    void GrabRod(BaseInteractionEventArgs pArgs) {
        ownedRod.transform.SetParent(null);
        ownedRod.GetComponent<Rigidbody>().isKinematic = false;
        Physics.IgnoreLayerCollision(rodLayer,pistolLayer, false);
        
    }

    void PutBackRod(BaseInteractionEventArgs pArgs) {
        ownedRod.transform.SetParent(transform);
        ownedRod.GetComponent<Rigidbody>().isKinematic = true;
        ownedRod.transform.localPosition = defaultRodPosition;
        ownedRod.transform.localRotation = defaultRodRotation;
        Physics.IgnoreLayerCollision(rodLayer, pistolLayer, true);
    }
    #endregion

}
