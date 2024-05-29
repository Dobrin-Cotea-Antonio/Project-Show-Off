using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MastUI : MonoBehaviour {

    [Header("Data")]
    [SerializeField] MastScript owner;
    [SerializeField] RectTransform hpBarTransform;

    #region Unity Events
    private void Awake() {
        owner.OnDamageTaken += DisplayHpBar;
    }
    #endregion

    #region Display
    void DisplayHpBar(float pHp, float pMaxHp) {
        float pPercentage = pHp / pMaxHp;
        hpBarTransform.localScale = new Vector3(pPercentage, 1, 1);
    }
    #endregion
}
