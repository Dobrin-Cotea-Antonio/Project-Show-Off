using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationCurvePair {
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
}

public class PlayerScript : MonoBehaviour {

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform mainCameraOffset;
    [SerializeField] AnimationCurvePair[] cameraShakeCurvePairs;

    [SerializeField] float cameraShakeDuration;
    [SerializeField] float cameraShakeUpdateIntervals;

    Vector3 mainCameraBaseOffset;

    private void Awake() {
        mainCameraBaseOffset = mainCameraOffset.localPosition;
    }

    public void StartShake(float pIntensity) {
        StartCoroutine(StartCameraShake(pIntensity));
    }

    IEnumerator StartCameraShake(float pIntensity) {

        float time = 0;

        while (time < cameraShakeDuration) {
            time += cameraShakeUpdateIntervals;
            yield return new WaitForSeconds(cameraShakeUpdateIntervals);
        }
        
    }

}
