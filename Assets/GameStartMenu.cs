using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class GameStartMenu : MonoBehaviour
{
    [Serializable]
    public class ButtonData
    {
        public Image buttonImage;
        public Sprite normalSprite; 
        public Sprite hoverSprite; 
    }

    [SerializeField] private List<ButtonData> buttons = new List<ButtonData>();

    private void Awake()
    {
        foreach (var buttonData in buttons)
        {
            if (buttonData.buttonImage == null)
            {
                Debug.LogError("Button Image component is not assigned.");
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeScene(string pScene)
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene(pScene);
        //StartCoroutine(LoadLevel(pScene));
    }

    IEnumerator LoadLevel(string pScene)
    {
        yield return 0;
        UnityEngine.SceneManagement.SceneManager.GetSceneByName(pScene);
    }

    public void OnPointerEnter(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= buttons.Count) return;

        var buttonData = buttons[buttonIndex];
        if (buttonData.buttonImage != null && buttonData.hoverSprite != null)
            buttonData.buttonImage.sprite = buttonData.hoverSprite;

        SoundManager.PlaySound(SoundManager.Sound.ButtonHover);
    }

    public void OnPointerExit(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= buttons.Count) return;

        var buttonData = buttons[buttonIndex];
        if (buttonData.buttonImage != null && buttonData.normalSprite != null)
            buttonData.buttonImage.sprite = buttonData.normalSprite;
    }
}