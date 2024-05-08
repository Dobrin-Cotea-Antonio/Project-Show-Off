using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReloadTrigger : MonoBehaviour {
    [Header("Data")]
    [SerializeField] PistolScript pistolAttachedTo;
    [SerializeField] string bulletTag;

    #region Unity Events
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag(bulletTag))
            return;

        Destroy(other.gameObject);
        pistolAttachedTo.PutBulletIn();
    }
    #endregion
}
