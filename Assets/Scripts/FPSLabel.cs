using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSLabel : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private float minFramerate = 9999999;

    private void Update()
    {
        textMesh.text =
            $"Current: {1 / Time.unscaledDeltaTime}\n" +
            $"Average: {Time.frameCount / Time.unscaledTime}\n" +
            $"Min: {Mathf.Min(1 / Time.unscaledDeltaTime, minFramerate)}";

        if (Time.frameCount > 300)
        {
            minFramerate = Mathf.Min(1 / Time.unscaledDeltaTime, minFramerate);
        }
    }
}
