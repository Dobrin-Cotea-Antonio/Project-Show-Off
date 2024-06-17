using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGlow : MonoBehaviour {

    [Header("Data")]
    [SerializeField] GameObject glow;

    public void Enable(bool pState) {
        glow.SetActive(pState);
    }
}
