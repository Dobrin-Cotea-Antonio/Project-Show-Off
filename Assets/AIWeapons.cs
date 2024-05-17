using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    WeaponIK weaponIK;
    Transform currentTarget;
    public float innacuracy = 0.4f;

    private void Start()
    {
        weaponIK = GetComponent<WeaponIK>();
        currentWeapon = GetComponentInChildren<RaycastWeapon>();
    }

    private void Update()
    {
        if (currentTarget)
        {
            Vector3 target = currentTarget.position + weaponIK.targetOffset;
            target += Random.insideUnitSphere * innacuracy;
            currentWeapon.UpdateWeapon(Time.deltaTime, target);
        }
    }

    public void SetFiring(bool enabled)
    {
        if (enabled)
        {
            currentWeapon.StartFiring();
        }
        else
        {
            currentWeapon.StopFiring();
        }
    }

    public void ActivateWeapon()
    {
        weaponIK.SetAimTransform(currentWeapon.raycastOrigin);
    }

    public void SetTarget(Transform target)
    {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }
}
