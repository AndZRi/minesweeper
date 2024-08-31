using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCounter : MonoBehaviour
{
    public static GameCounter Instance;

    public int flags = 0;
    public int openedCells = 0;
    
    private StatsWidget statsWidget;

    private List<Cell> frameOpenedCells = new();

    private void Awake() => Instance = this;

    void Start()
    {
        UIManager.Instance.OnManagerStarted.AddListener(UIManagerStart);
        UIManager.Instance.GetWidget(out statsWidget);
        statsWidget.Show();
    }

    private void UIManagerStart()
    {
        statsWidget.Show();
    }

    private void LateUpdate()
    {
        if (frameOpenedCells.Count > 0)
        {
            ActivateEffects();
            openedCells += frameOpenedCells.Count;
            statsWidget.cellsCounter.SetText(openedCells);
            frameOpenedCells.Clear();
        }
    }

    private void ActivateEffects()
    {
        foreach (var cell in frameOpenedCells)
        {
            CellEffects cellEffects = cell.gameObject.GetComponent<CellEffects>();
            RarityLevel level = RarityLevel.GetRarityLevelByCells(frameOpenedCells.Count);
            cellEffects.SummonDigVFX(level);
        }
    }

    public void CellOpened(Cell sender)
    {
        frameOpenedCells.Add(sender);
    }

    public void CellFlagged(bool flagged)
    {
        if (flagged)
            flags += 1;
        else
            flags -= 1;

        statsWidget.flagsCounter.SetText(flags);
    }

    public void ResetCounter()
    {
        flags = 0;
        openedCells = 0;
        statsWidget.Reset();
    }
}
