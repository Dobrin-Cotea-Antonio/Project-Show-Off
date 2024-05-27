using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileScript : MonoBehaviour {

    [Header("Bullet Data")]
    [SerializeField] float _speed;
    [SerializeField] float _damage;

    public float speed { get { return _speed; } set { speed = value; } }
    public float damage { get { return _damage; } set { damage = value; } }

    Rigidbody rb;
    Vector3 initialPosition;
    float distanceToDestroy = 50;

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
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
            damagable.TakeDamage(damage);

        Destroy(gameObject);
    }
    #endregion
}
