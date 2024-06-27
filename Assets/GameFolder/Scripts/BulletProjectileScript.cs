using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectileScript : MonoBehaviour
{
    [Header("Bullet Data")] [SerializeField]
    float _speed;

    [SerializeField] float _damage;

    public float speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    Vector3 lastPosition;

    #region Unity Events

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(lastPosition, transform.forward, out hit,
                Vector3.Distance(lastPosition, transform.position)))
        {
            {
                IDamagable damagable = hit.collider.GetComponent<IDamagable>();

                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                    Destroy(gameObject);
                }
                
                Destroy(gameObject);
            }
        }
        
        lastPosition = transform.position;
        
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
            damagable.TakeDamage(damage);

        Destroy(gameObject);
    }*/

    #endregion

    #region Helper Methods

    public void SetDirection(Vector3 pDirection)
    {
        transform.forward = pDirection;
    }

    #endregion
}