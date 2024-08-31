using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isGameActive = false;

    private void Awake() => instance = this;

    private void Start()
    {
        UIManager.Instance.OnManagerStarted.AddListener(UIManagerStarted);
        UIManager.Instance.Show<MainMenuWidget>();
    }

    private void UIManagerStarted()
    {
        UIManager.Instance.Show<MainMenuWidget>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.GetWidget(out GameMenuWidget widget);
            if (!widget.gameObject.activeSelf)
            {
                UIManager.Instance.Show<GameMenuWidget>();
                isPaused = true;
            }
            else
            {
                UIManager.Instance.Hide<GameMenuWidget>();
                isPaused = false;
            }
        }
    }

    public void GameOver()
    {
        isPaused = true;
        UIManager.Instance.Show<GameOverWidget>();
    }

    public void StartGame()
    {
        Field.Instance.StartGame();
        CameraController.Instance.ResetPosition();
        isGameActive = true;
    }

    public void QuitToMainMenu()
    {
        isGameActive = false;
        Field.Instance.ClearField();
        GameCounter.Instance.ResetCounter();
        UIManager.Instance.HideAll();
        UIManager.Instance.Show<MainMenuWidget>();
        isPaused = false;
    }

    public void RestartGame()
    {
        Field.Instance.ResetField();
        CameraController.Instance.ResetPosition();
        isPaused = false;
    }
}
