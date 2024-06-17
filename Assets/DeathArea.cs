using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathArea : MonoBehaviour {


    #region Unity Events
    private void Start() {
        SceneManager.instance.hook.OnGrab+=Disable;
        SceneManager.instance.hook.OnRelease+=Enable;
    }

    private void OnTriggerEnter(Collider other) {
        if (!enabled)
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
            damagable.TakeDamage(999999);
    }
    #endregion

    #region Helper Methods
    private void Enable() {
        enabled = true;
    }

    private void Disable() {
        enabled = false;
    }
    #endregion
}
