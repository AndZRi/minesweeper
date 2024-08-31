using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Widget : MonoBehaviour
{
    public abstract void Initialize();

    public virtual void Show() => gameObject.SetActive(true);

    public virtual void Hide() => gameObject.SetActive(false);
}
