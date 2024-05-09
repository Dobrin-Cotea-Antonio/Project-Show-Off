using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileScript : MonoBehaviour {

    [Header("Bullet Data")]
    [SerializeField] float speed;
    [SerializeField] float damage;

    Rigidbody rb;
    Vector3 initialPosition;
    float distanceToDestroy=200;

    #region Unity Events
    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
        initialPosition = transform.position;
    }

    private void Update() {
        if ((transform.position - initialPosition).magnitude >= distanceToDestroy)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
    #endregion

}
