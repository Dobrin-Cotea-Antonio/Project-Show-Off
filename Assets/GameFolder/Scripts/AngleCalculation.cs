using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleCalculation : MonoBehaviour {

    [Header("Data")]
    [SerializeField] ArchedProjectileShooter shooter;
    [SerializeField] Transform targetObj;
    [SerializeField] Transform gunObj;

    static float stepSize;

    private LineRenderer lineRenderer;

    #region Unity Events
    void Awake() {
        stepSize = Time.fixedDeltaTime * 1f;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update() {
        RotateGun();
        DrawTrajectoryPath();
    }
    #endregion

    #region Rotation
    void RotateGun() {
        float highAngle = 0f;
        float lowAngle = 0f;

        bool canContinue = CalculateAngleToHitTarget(out highAngle, out lowAngle);

        float angle = (float)highAngle;

        //Are we in range
        if (canContinue) {
            gunObj.localEulerAngles = new Vector3(360f - angle, 0f, 0f);
            transform.LookAt(targetObj);
            transform.eulerAngles = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }

    bool CalculateAngleToHitTarget(out float theta1, out float theta2) {
        //Initial speed
        float v = shooter.bulletSpeed;

        Vector3 targetVec = targetObj.position - gunObj.position;

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

    #region Trajectory Display
    void DrawTrajectoryPath() {
        float timeToHitTarget = CalculateTimeToHitTarget();

        int maxIndex = Mathf.RoundToInt(timeToHitTarget / stepSize);

        lineRenderer.positionCount = maxIndex;

        //Start values
        Vector3 currentVelocity = gunObj.transform.forward * shooter.bulletSpeed;
        Vector3 currentPosition = gunObj.transform.position;

        Vector3 newPosition = Vector3.zero;
        Vector3 newVelocity = Vector3.zero;

        //Build the trajectory line
        for (int index = 0; index < maxIndex; index++) {
            lineRenderer.SetPosition(index, currentPosition);

            //Calculate the new position of the bullet
            AngleCalculation.CurrentIntegrationMethod(stepSize, currentPosition, currentVelocity, out newPosition, out newVelocity);

            currentPosition = newPosition;
            currentVelocity = newVelocity;
        }
    }

    public float CalculateTimeToHitTarget() {
        Vector3 currentVelocity = gunObj.transform.forward * shooter.bulletSpeed;
        Vector3 currentPosition = gunObj.transform.position;

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

    #region Physics prediction
    public static void CurrentIntegrationMethod(
        float h,
        Vector3 currentPosition,
        Vector3 currentVelocity,
        out Vector3 newPosition,
        out Vector3 newVelocity) {
        //IntegrationMethods.EulerForward(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        //IntegrationMethods.Heuns(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        //IntegrationMethods.RungeKutta(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
        IntegrationMethods.BackwardEuler(h, currentPosition, currentVelocity, out newPosition, out newVelocity);
    }

    public class IntegrationMethods : MonoBehaviour {
        public static void BackwardEuler(
            float h,
            Vector3 currentPosition,
            Vector3 currentVelocity,
            out Vector3 newPosition,
            out Vector3 newVelocity) {
            //Init acceleration
            //Gravity
            Vector3 acceleartionFactor = Physics.gravity;

            //Main algorithm
            newVelocity = currentVelocity + h * acceleartionFactor;

            newPosition = currentPosition + h * newVelocity;
        }
    }
    #endregion
}