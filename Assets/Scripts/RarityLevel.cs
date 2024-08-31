using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity Level", menuName = "Rarity Level")]
public class RarityLevel : ScriptableObject
{
    private static RarityLevel[] rarities;

    public string displayName;
    public int cellsNeed;
    public Color color;
    public float fadeTime;

    public static void LoadRarities()
    {
        rarities = Resources.LoadAll<RarityLevel>("RarityLevels");
        Array.Sort(rarities, (RarityLevel x, RarityLevel y) => -x.cellsNeed.CompareTo(y.cellsNeed));
    }

    public static RarityLevel GetRarityLevelByCells(int cells)
    {
        if (rarities == null)
            LoadRarities();

        foreach (RarityLevel level in rarities)
        {
            if (level.cellsNeed <= cells)
                return level;
        }
        return null;
    }
}
