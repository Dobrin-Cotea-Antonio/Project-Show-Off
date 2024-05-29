using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour {
    [Header("Data")]
    [SerializeField] EnemyAI owner;

    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;

    [SerializeField] Image foregroundImage;
    [SerializeField] Image backgroundImage;

    #region Unity Events
    private void Awake() {
        owner.OnDamageTaken += UpdateHpBar;
        owner.OnDeath += DisableHpBar;
    }
    #endregion

    #region Hp Bar
    private void UpdateHpBar(float pCurrentHp, float pMaxHp) {
        float percentage = pCurrentHp / pMaxHp;

        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
        foregroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    private void DisableHpBar(EnemyAI pEnemy) {
        gameObject.SetActive(false);
    }
    #endregion
}
