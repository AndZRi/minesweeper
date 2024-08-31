using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Vector2IntExtensions
{
    public static Vector2Int[] GetDirections()
    {
        return new Vector2Int[] {
            Vector2Int.up + Vector2Int.left,
            Vector2Int.up,
            Vector2Int.up + Vector2Int.right,
            Vector2Int.right,
            Vector2Int.down + Vector2Int.right,
            Vector2Int.down,
            Vector2Int.down + Vector2Int.left,
            Vector2Int.left
        };
    }
}
