using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuWidget : Widget
{
    public Button PlayButton;
    public Button SettingsButton;
    public Button QuitButton;

    public override void Initialize()
    {
        QuitButton.onClick.AddListener(QuitGame);
        PlayButton.onClick.AddListener(ShowGameModeMenu);
    }

    public void ShowGameModeMenu()
    {
        UIManager.Instance.Show<GameModeMenu>();
    }

    public void QuitGame()
    {
        Application.Quit();
        print("QUITTED?!?!?!");
    }
}
