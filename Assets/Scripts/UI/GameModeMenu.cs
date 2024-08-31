using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameModeMenu : Widget
{
    public Button PlayButton;
    public TMP_InputField WidthInput;
    public TMP_InputField HeightInput;
    public TMP_InputField MinesInput;

    public override void Initialize()
    {
        PlayButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        if (!SetFieldParameters())
            return;

        UIManager.Instance.Hide<MainMenuWidget>();
        GameManager.instance.StartGame();
        Hide();
    }
    
    private bool SetFieldParameters()
    {
        int width = int.Parse(WidthInput.text);
        int height = int.Parse(HeightInput.text);
        int mines = int.Parse(MinesInput.text);

        if (width <= 3 || height <= 3)
            return false;

        Field.Instance.FieldWidth = width;
        Field.Instance.FieldHeight = height;
        Field.Instance.MinesCount = mines;
        
        return true;
    }
}
