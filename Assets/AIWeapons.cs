using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapons : MonoBehaviour
{
    RaycastWeapon currentWeapon;
    WeaponIK weaponIK;
    Transform currentTarget;

    private void Start()
    {
        weaponIK = GetComponent<WeaponIK>();
        currentWeapon = GetComponentInChildren<RaycastWeapon>();
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
