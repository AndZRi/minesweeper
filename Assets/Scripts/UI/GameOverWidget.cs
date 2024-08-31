using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWidget : Widget
{
    public Button RetryButton;
    public Button QuitButton;

    public override void Initialize()
    {
        RetryButton.onClick.AddListener(Retry);
        QuitButton.onClick.AddListener(Quit);
    }

    public void Retry()
    {
        GameManager.instance.RestartGame();
        GameCounter.Instance.ResetCounter();
        UIManager.Instance.Hide<GameOverWidget>();
    }

    public void Quit()
    {
        GameManager.instance.QuitToMainMenu();
    }
}
