using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodReloadTrigger : MonoBehaviour {
    [Header("Data")]
    [SerializeField] PistolScript pistolAttachedTo;
    [SerializeField] string rodTag;

    #region Unity Events
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag(rodTag))
            return;

        pistolAttachedTo.Reload();
    }
    #endregion
}
