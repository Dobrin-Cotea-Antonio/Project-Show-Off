using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBarrel : MonoBehaviour, IDamagable
{
    [Header("Explosion Data")] [SerializeField]
    float explosionRadius = 5f;
    [SerializeField] float explosionDamage = 100f;
    [SerializeField] float explosionForce = 1000f;
    [SerializeField] LayerMask damageableLayers;
    [SerializeField] GameObject explosionEffect;

    public void TakeDamage(float damage)
    {
        Explode();
    }

    private void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            SoundManager.PlaySound(SoundManager.Sound.Explosion, transform);
            
            Destroy(explosion, 2f);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, damageableLayers);

        foreach (Collider hitCollider in hitColliders)
        {
            IDamagable damagable = hitCollider.GetComponent<IDamagable>();
            if (damagable != null)
            {
                float proximity = (transform.position - hitCollider.transform.position).magnitude;
                float effect = 1 - (proximity / explosionRadius);
                damagable.TakeDamage(explosionDamage * effect);
            }
            
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}