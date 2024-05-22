using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class BulletIndicatorScript : MonoBehaviour {
    public Action OnCorrectInteraction;
    public Action OnIncorrectInteraction;

    [Header("Indicator Data")]
    [SerializeField] [Range(0, 1)] float indicatorSize;
    [SerializeField] [Range(0, 1)] float indicatorLocation;

    [Header("Moving Line Data")]
    [SerializeField] [Range(0, 1)] float lineSize;
    [Tooltip("Speed of the line over one second. If speed is 1 then the line will move from one side to the other in 1 second")]
    [SerializeField] [Range(0, 1)] float lineSpeed;

    float linePosition = 0;
    float maxLinePostion = 1;

    [Header("Indicators Colors")]
    [SerializeField] Color outsideIndicatorColor;
    [SerializeField] Color insideIndicatorColor;
    [SerializeField] Color lineColor;

    [Header("Indicator Material")]
    [SerializeField] Material indicatorMaterial;

    #region Unity Events
    private void Update() {
        UpdateLineLocation();
        UpdateShaderValues();
        CheckIfLineIsInDeadzone();
    }
    #endregion

    #region Shader Updates
    void UpdateLineLocation() {
        maxLinePostion = 1 - lineSize;

        linePosition = Mathf.Clamp(linePosition + lineSpeed * Time.deltaTime, 0, maxLinePostion);

        if (linePosition == 0 || linePosition == maxLinePostion)
            lineSpeed *= -1;
    }

    void UpdateShaderValues() {
        indicatorMaterial.SetFloat("_StartValue", indicatorLocation);
        indicatorMaterial.SetFloat("_EndValue", Mathf.Clamp(indicatorLocation + indicatorSize, 0, 1));
        indicatorMaterial.SetFloat("_LineStart", linePosition);
        indicatorMaterial.SetFloat("_LineEnd", Mathf.Clamp(linePosition + lineSize, 0, 1));
        indicatorMaterial.SetColor("_Color1", outsideIndicatorColor);
        indicatorMaterial.SetColor("_Color2", insideIndicatorColor);
        indicatorMaterial.SetColor("_Color3", lineColor);
    }
    #endregion

    #region Input Detection
    public void CheckIfLineIsInDeadzone() {
        //introduce correct input checking (maybe use an event?)

        if (!Input.GetKeyDown(KeyCode.X))
            return;

        if ((linePosition >= indicatorLocation && (linePosition + lineSize) <= (indicatorLocation + indicatorSize))) {
            OnCorrectInteraction?.Invoke();
            Debug.Log("correct");
        } else {
            OnIncorrectInteraction?.Invoke();
            Debug.Log("incorrect");
        }
    }
    #endregion
}
