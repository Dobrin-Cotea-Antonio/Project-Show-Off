using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchedProjectileShooter : MonoBehaviour {

    [Header("Data")]
    [SerializeField] float _bulletSpeed;
    [SerializeField] GameObject bulletObj;
    [SerializeField] Transform bulletParent;

    public float bulletSpeed { get { return _bulletSpeed; } }

    #region Unity Events
    void Start() {
        StartCoroutine(FireBullet());
    }
    #endregion

    #region Fire
    public IEnumerator FireBullet() {
        while (true) {
            GameObject newBullet = Instantiate(bulletObj, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = _bulletSpeed * transform.forward;
            yield return new WaitForSeconds(2f);
        }
    }
    #endregion
}
