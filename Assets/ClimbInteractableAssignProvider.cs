using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractableAssignProvider : MonoBehaviour {

    #region Unity Events
    private void Start() {
        ClimbProvider climbProvider = SceneManager.instance.playerGameObject.GetComponent<ClimbProvider>();
        ClimbInteractable climbInteractable = GetComponent<ClimbInteractable>();

        climbInteractable.climbProvider = climbProvider;
    }
    #endregion

}
