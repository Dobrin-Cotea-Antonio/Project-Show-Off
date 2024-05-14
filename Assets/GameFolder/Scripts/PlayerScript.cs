using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationCurvePair {
    //values inside should be kept between -0.5 and 0.5
    //initial time is 0 and final time is 1 and their values are 0
    //the values are represented by an intensity of 1
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
}

public class PlayerScript : MonoBehaviour {

    [Header("Camera Shake")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform mainCameraOffsetTransform;
    [SerializeField] AnimationCurvePair[] cameraShakeCurvePairs;
    [SerializeField] float cameraShakeUpdateIntervals;

    #region Camera Shake
    public void StartShake(float pIntensity, float pDuration) {    //Intensity should be between 0 and 1 
        StartCoroutine(StartCameraShake(pIntensity, pDuration));
    }

    IEnumerator StartCameraShake(float pIntensity, float pDuration) {
        float time = 0;
        Vector3 previousOffset = Vector3.zero;
        Vector3 offset = Vector3.zero;
        int randomCurve = Random.Range(0, cameraShakeCurvePairs.Length);

        while (time < pDuration) {
            time += cameraShakeUpdateIntervals;
            float pPercentageDone = time / pDuration;

            previousOffset = offset;
            offset = new Vector3(cameraShakeCurvePairs[randomCurve].xCurve.Evaluate(pPercentageDone), cameraShakeCurvePairs[randomCurve].yCurve.Evaluate(pPercentageDone), 0) * pIntensity;
            mainCameraOffsetTransform.localPosition += (offset - previousOffset);

            yield return new WaitForSeconds(cameraShakeUpdateIntervals);
        }
    }
    #endregion
}
