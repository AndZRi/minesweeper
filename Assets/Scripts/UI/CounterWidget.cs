using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterWidget : Widget
{
    public TextMeshProUGUI textMesh;

    public override void Initialize()
    {
        textMesh.text = "0";
    }

    public void SetText(int value)
    {
        textMesh.SetText(value.ToString());
    }
}
