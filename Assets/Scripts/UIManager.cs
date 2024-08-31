using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [HideInInspector] public UnityEvent OnManagerStarted = new();

    [SerializeField] private Widget[] widgets;

    public bool GetWidget<T>(out T widget) where T : Widget
    {
        foreach (var i in widgets)
        {
            if (i is T x)
            {
                widget = x;
                return true;
            }
        }

        widget = null;
        return false;
    }

    public void Show<T>() where T : Widget
    {
        if (!GetWidget(out T widget))
            return;

        widget.Show();
        print(widget.name + " is shown");
    }

    public void Hide<T>() where T : Widget
    {
        if (!GetWidget(out T widget))
            return;

        widget.Hide();
        print(widget.name + " is hidden");
    }

    public void HideAll()
    {
        foreach (var i in widgets)
        {
            i.Hide();
        }
    }

    private void Awake() => Instance = this;

    private void Start()
    {
        foreach (var i in widgets)
        {
            i.Initialize();
            print(i.name + " initialized");
            i.Hide();
        }
        
        OnManagerStarted.Invoke();
    }
}
