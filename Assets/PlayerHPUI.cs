using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour {

    [Header("HP")]
    [SerializeField] Image hp;

    #region Unity Events
    private void Start() {
        SceneManager.instance.playerGameObject.GetComponent<Player>().OnDamageTaken += UpdateHp;
    }
    #endregion 

    #region Hp
    void UpdateHp(float pCurrentHp, float pMaxHp) {
        float percentage = pCurrentHp / pMaxHp;
        hp.rectTransform.localScale = new Vector3(percentage, hp.rectTransform.localScale.y, hp.rectTransform.localScale.z);
    }
    #endregion
}
