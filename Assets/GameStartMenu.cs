using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartMenu : MonoBehaviour {

    public void QuitGame() {
        Application.Quit();
    }

    public void ChangeScene(string pScene) {
        UnityEngine.SceneManagement.SceneManager.LoadScene(pScene);
    }
}
