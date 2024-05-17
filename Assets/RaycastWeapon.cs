using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }
    public Transform raycastOrigin;
    
    public bool isFiring = false;
    public float fireRate = 25f;
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0f;
    public int maxBounces = 0;
    public bool debug = false;
    public string weaponName;

    public int ammoCount = 9999999;
    public int clipSize;
    public float damage = 10f;
    
    public TrailRenderer tracerEffect;
    public ParticleSystem impactEffect;
    

    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    List<Bullet> bullets = new List<Bullet>();
    float maxLifeTime = 3.0f;

    Vector3 GetPosition(Bullet bullet)
    {
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialPosition * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        bullet.bounce = maxBounces;
        return bullet;
    }
    
    public void StartFiring()
    {
        isFiring = true;
        if (accumulatedTime > 0)
        {
            accumulatedTime = 0;
        }
    }

    public void UpdateWeapon(float deltaTime, Vector3 target)
    {
        if (isFiring)
        {
            UpdateFiring(deltaTime, target);
        }
        
        accumulatedTime += deltaTime;
        
        UpdateBullets(deltaTime);
        
    }

    private void UpdateBullets(float deltaTime)
    {
        SimulateBullets(deltaTime);
        DestroyBullets();
    }

    private void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    private void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet);
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet);
            RaycastSegment(p0, p1, bullet);
        });
    }

    private void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            impactEffect.transform.position = hitInfo.point;
            impactEffect.transform.forward = hitInfo.normal;
            impactEffect.Emit(1);
        }
    }

    private void UpdateFiring(float deltaTime, Vector3 target)
    {
        float fireInterval = 1.0f / fireRate;
        while (accumulatedTime >= 0)
        {
            FireBullet(target);
            accumulatedTime -= fireInterval;
        }
    }

    private void FireBullet(Vector3 target)
    {
        if(ammoCount <= 0) return;
        ammoCount--;
        
        Vector3 velocity = (target - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);
    }

    public void StopFiring()
    {
        isFiring = false;
    }
    
    
}
