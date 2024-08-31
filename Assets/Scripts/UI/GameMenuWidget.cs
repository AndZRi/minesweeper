using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuWidget : Widget
{
    public Button ResumeButton;
    public Button SettingsButton;
    public Button QuitButton;

    public override void Initialize()
    {
        ResumeButton.onClick.AddListener(ResumeGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    private void ResumeGame()
    {
        GameManager.instance.isPaused = false;
        UIManager.Instance.Hide<GameMenuWidget>();
    }

    private void QuitGame()
    {
        Application.Quit();
        print("QUITTED?!?!?!");
    }
}
