using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PistolScript : MonoBehaviour{

    [Header("Pistol Data")]
    [SerializeField] Transform shootPoint;

    [Header("Bullet Data")]
    [SerializeField] GameObject bulletPrefab;

    XRGrabInteractable interactableComponent;

    #region Unity Events
    private void Awake() {
        interactableComponent = GetComponent<XRGrabInteractable>();
        interactableComponent.activated.AddListener(Shoot);
    }

    #endregion

    #region Shooting 
    void Shoot(ActivateEventArgs pArgs) {
        GameObject bulletGameobject = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    }
    #endregion

}
