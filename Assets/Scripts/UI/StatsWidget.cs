using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsWidget : Widget
{
    public CounterWidget flagsCounter;
    public CounterWidget cellsCounter;

    public override void Initialize()
    {
        flagsCounter.Initialize();
        cellsCounter.Initialize();
    }

    public void Reset()
    {
        flagsCounter.SetText(0);
        cellsCounter.SetText(0);
    }
}
