using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    private void OnTriggerEnter(Collider pOther) {
        IDamagable damagable = pOther.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
            damagable.TakeDamage(999999);
    }
}
