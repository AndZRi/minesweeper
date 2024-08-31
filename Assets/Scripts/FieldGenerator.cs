using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;

static public class FieldGenerator
{
    private static int whileLimit = 10000;
    private static int safeRadius = 1;

    public static byte[,] GenerateMap(int fieldWidth, int fieldHeight, int mines, Vector2Int InitialPosition)
    {
        byte[,] currentMap = new byte[fieldHeight, fieldWidth];
        // Заполняем нулями
        for (int y = 0; y < fieldHeight; y++)
        {
            for (int x = 0; x < fieldWidth; x++)
            {
                currentMap[y, x] = 0;
            }
        }


        int minesLeft = mines;
        int i = 0;
        while (minesLeft > 0)
        {
            i++;
            if (i > whileLimit)
            {
                Debug.LogError("Too many iterations of while loop when generating a field");
                break;
            }

            int x = UnityEngine.Random.Range(0, fieldWidth);
            int y = UnityEngine.Random.Range(0, fieldHeight);

            if (currentMap[y, x] == 1)
                continue;
            if (Math.Abs(x - InitialPosition.x) <= safeRadius && Math.Abs(y - InitialPosition.y) <= safeRadius)
                continue;

            currentMap[y, x] = 1;
            minesLeft--;
        }

        return currentMap;
    }
}
