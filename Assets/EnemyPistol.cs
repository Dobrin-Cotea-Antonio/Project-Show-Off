using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol : EnemyWeapon {

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootPoint;

    #region Shooting
    public override void Shoot(Vector3 pTarget) {

        if (Time.time - lastTimeShot < data.delayBetweenShots)
            return;

        float randomFloat = Random.Range(0f, 1f);

        if (randomFloat >= data.chanceToShootAfterDelay) {
            lastTimeShot = Time.time;
            return;
        }

        CreateBullet(pTarget);
    }

    void CreateBullet(Vector3 pTarget) {
        OnSuccessfulShot?.Invoke();

        GameObject bulletObject = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        BulletProjectileScript bullet = bulletObject.GetComponent<BulletProjectileScript>();
        bullet.damage = data.damage;
        bullet.speed = data.bulletSpeed;
        lastTimeShot = Time.time;
        bullet.SetDirection((pTarget - bulletObject.transform.position).normalized);
    }
    #endregion
}
