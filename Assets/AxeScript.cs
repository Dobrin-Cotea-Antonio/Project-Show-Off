using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : EnemyWeapon {

    [SerializeField] GameObject axePrefab;
    [SerializeField] Transform throwPoint;

    Transform targetObj;

    float stepSize;

    #region Unity Events
    private void Start() {
        targetObj = SceneManager.instance.playerGameObject.transform;
        stepSize = Time.fixedDeltaTime;
    }

    void Update() {
        RotateGun();
    }
    #endregion

    #region Shooting
    public override void Shoot(Vector3 pTarget) {
        if (Time.time - lastTimeShot < data.delayBetweenShots)
            return;

        float randomFloat = Random.Range(0f, 1f);

        if (randomFloat >= data.chanceToShootAfterDelay) {
            lastTimeShot = Time.time;
            return;
        }

        CreateAxe(pTarget);
    }

    void CreateAxe(Vector3 pTarget) {
        GameObject bulletObject = Instantiate(axePrefab, throwPoint.position, Quaternion.identity);

        BulletProjectileScript bullet = bulletObject.GetComponent<BulletProjectileScript>();
        bullet.damage = data.damage;
        bullet.speed = data.bulletSpeed;
        lastTimeShot = Time.time;
        bullet.SetDirection(throwPoint.forward);
    }
    #endregion

    #region Rotation
    void RotateGun() {
        float highAngle = 0f;
        float lowAngle = 0f;

        bool canContinue = CalculateAngleToHitTarget(out highAngle, out lowAngle);

        float angle = (float)highAngle;

        if (canContinue) {
            throwPoint.localEulerAngles = new Vector3(360f - angle, 0f, 0f);
            transform.LookAt(targetObj);
            transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    bool CalculateAngleToHitTarget(out float theta1, out float theta2) {
        float v = data.bulletSpeed;

        Vector3 targetVec = targetObj.position - throwPoint.position;

        //Vertical distance
        float y = targetVec.y;

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
        float x = targetVec.magnitude;

        //Gravity
        float g = Mathf.Abs(Physics.gravity.y);

        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

        //Check if we are within range
        if (underTheRoot >= 0f) {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
            return true;
        } else {
            theta1 = 0;
            theta2 = 0;
            return false;
        }
    }
    #endregion

    #region Physics prediction
    public static void CurrentIntegrationMethod(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity) {
        IntegrationMethods.BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    }

    public class IntegrationMethods : MonoBehaviour {
        public static void BackwardEuler(
            float h,
            Vector3 currentPosition,
            Vector3 currentVelocity,
            out Vector3 newPosition,
            out Vector3 newVelocity) {

            Vector3 acceleartionFactor = Physics.gravity;

            newVelocity = currentVelocity + h * acceleartionFactor;

            newPosition = currentPosition + h * newVelocity;
        }
    }
    #endregion

    #region Height Prediction
    public Vector3 ReturnMaxThrowHeight() {
        float timeToHitTarget = CalculateTimeToHitTarget();

        int maxIndex = Mathf.RoundToInt(timeToHitTarget / stepSize);

        Vector3 currentVelocity = throwPoint.transform.forward * data.bulletSpeed;
        Vector3 currentPosition = throwPoint.transform.position;

        Vector3 targetPos = currentPosition;

        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        for (int i = 0; i < maxIndex; i++) {

            AngleCalculation.CurrentIntegrationMethod(stepSize, currentPosition, currentVelocity, out newPosition, out newVelocity);

            currentPosition = newPosition;
            currentVelocity = newVelocity;

            if (currentPosition.y > targetPos.y)
                targetPos = currentPosition;
        }

        return targetPos;
    }

    float CalculateTimeToHitTarget() {
        Vector3 currentVelocity = throwPoint.transform.forward * data.bulletSpeed;
        Vector3 currentPosition = throwPoint.transform.position;

        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        float time = 0f;

        for (time = 0f; time < 30f; time += stepSize) {
            AngleCalculation.CurrentIntegrationMethod(stepSize, currentPosition, currentVelocity, out newPosition, out newVelocity);

            if (newPosition.y < currentPosition.y && newPosition.y < targetObj.position.y) {
                time += stepSize * 2f;

                break;
            }

            currentPosition = newPosition;
            currentVelocity = newVelocity;
        }

        return time;
    }
    #endregion
}
