using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileScript : MonoBehaviour {

    [Header("Bullet Data")]
    [SerializeField] float speed;
    [SerializeField] float damage;

    Rigidbody rb;

    #region Unity Events
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Destroyed");
        Destroy(gameObject);
    }
    #endregion

}
