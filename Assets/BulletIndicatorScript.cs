using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class BulletIndicatorScript : MonoBehaviour {
    //known bug: all pistols will display the same amount of ammo/reload animation

    public Action OnCorrectInteraction;
    public Action OnIncorrectInteraction;
    public Action OnFinish;

    [Header("Indicator Data")]
    [SerializeField] [Range(0, 1)] float indicatorSize;
    [SerializeField] [Range(0, 1)] float indicatorLocation;

    [Header("Moving Line Data")]
    [SerializeField] [Range(0, 1)] float lineSize;
    [Tooltip("Speed of the line over one second. If speed is 1 then the line will move from one side to the other in 1 second")]
    [SerializeField] [Range(0, 1)] float lineSpeed;
    float defaultLineSpeed;

    float linePosition = 0;

    [Header("Indicators Colors")]
    [SerializeField] [ColorUsageAttribute(true, true)] Color outsideIndicatorColor;
    [SerializeField] [ColorUsageAttribute(true, true)] Color insideIndicatorColor;
    [SerializeField] [ColorUsageAttribute(true, true)] Color lineColor;

    [SerializeField] [ColorUsageAttribute(true, true)] Color ammoColor;
    [SerializeField] [ColorUsageAttribute(true, true)] Color emptyColor;

    [Header("Indicator Material")]
    [SerializeField] Material indicatorMaterial;
    [SerializeField] Material bulletDisplayMaterial;

    MeshRenderer meshRenderer;
    bool isInReloadMode = false;
    float percentageAmmo = 1;

    #region Unity Events
    private void Awake() {
        defaultLineSpeed = lineSpeed;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        SelectRandomIndicatorLocation();
    }

    private void Update() {
        UpdateLineLocation();
        UpdateShaderValues();
    }
    #endregion

    #region Shader Updates
    void UpdateLineLocation() {
        if (!isInReloadMode)
            return;

        float maxLinePostion = 1 - lineSize;

        linePosition = Mathf.Clamp(linePosition + lineSpeed * Time.deltaTime, 0, maxLinePostion);

        if (linePosition == maxLinePostion)
            OnFinish?.Invoke();


        //if (linePosition == 0 || linePosition == maxLinePostion)
        //    lineSpeed *= -1;
    }

    void UpdateShaderValues() {
        indicatorMaterial.SetFloat("_StartValue", indicatorLocation);
        indicatorMaterial.SetFloat("_EndValue", Mathf.Clamp(indicatorLocation + indicatorSize, 0, 1));
        indicatorMaterial.SetFloat("_LineStart", linePosition);
        indicatorMaterial.SetFloat("_LineEnd", Mathf.Clamp(linePosition + lineSize, 0, 1));
        indicatorMaterial.SetColor("_Color1", outsideIndicatorColor);
        indicatorMaterial.SetColor("_Color2", insideIndicatorColor);
        indicatorMaterial.SetColor("_Color3", lineColor);

        bulletDisplayMaterial.SetColor("_Color1", ammoColor);
        bulletDisplayMaterial.SetColor("_Color2", emptyColor);
        bulletDisplayMaterial.SetFloat("_BulletPercentageValue", percentageAmmo);
    }
    #endregion

    #region Input Detection
    public void CheckIfLineIsInDeadzone() {
        //(linePosition >= indicatorLocation && (linePosition + lineSize) <= (indicatorLocation + indicatorSize))

        if ((linePosition + lineSize >= indicatorLocation && (linePosition) <= (indicatorLocation + indicatorSize))) {
            OnCorrectInteraction?.Invoke();
        } else {
            OnIncorrectInteraction?.Invoke();
        }
    }
    #endregion

    #region Indicator Location
    public void SelectRandomIndicatorLocation(bool pResetLinePosition = true) {
        indicatorLocation = UnityEngine.Random.Range(0, 1 - indicatorSize);
        if (pResetLinePosition)
            linePosition = 0;
    }
    #endregion

    #region Helper Methods
    public void EnableReloadMode(bool pState) {
        if (pState) {
            meshRenderer.material = indicatorMaterial;
            SelectRandomIndicatorLocation();
            isInReloadMode = true;
            UpdateSpeed(1);
        } else {
            meshRenderer.material = bulletDisplayMaterial;
            isInReloadMode = false;
        }
    }

    public void UpdatePercentage(float pValue) {
        percentageAmmo = pValue;
    }

    public void UpdateSpeed(float pPercentage) {
        lineSpeed = defaultLineSpeed * pPercentage;
        //Debug.Log("slowed");
    }
    #endregion
}
