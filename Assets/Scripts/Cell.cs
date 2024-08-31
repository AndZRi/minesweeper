using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [Flags]
    public enum States
    {
        None,
        Hidden,
        Shown,
        Flagged,
        Markered,
        BeingPressed,
        AbleToDig = Hidden | BeingPressed
    }
    
    public UnityEvent<Cell> OnCellOpened = new();
    public UnityEvent<bool> OnCellFlagged = new UnityEvent<bool>();

    [SerializeField] private Sprite[] DigitsSprites;
    [SerializeField] private Sprite MineSprite;
    [SerializeField] private Sprite FlagSprite;
    [SerializeField] private Sprite HiddenSprite;
    [SerializeField] private Sprite PressedSprite;
    [SerializeField] private Sprite MarkeredSprite;

    public Field ParentField;
    public Vector2Int Position;

    private States currentState = States.Hidden;
    public States CurrentState
    {
        get { return currentState; }
        set
        {
            switch (value)
            {
                case States.Hidden:
                    currentState = States.Hidden;
                    spriteRenderer.sprite = HiddenSprite;
                    break;

                case States.Shown:
                    currentState = States.Shown;
                    if (ParentField.IsMine(Position))
                        spriteRenderer.sprite = MineSprite;
                    else
                        spriteRenderer.sprite = DigitsSprites[mines];
                    OnCellOpened.Invoke(this);
                    break;

                case States.Flagged:
                    currentState = States.Flagged;
                    spriteRenderer.sprite = FlagSprite;
                    OnCellFlagged.Invoke(true);
                    break;

                case States.Markered:
                    currentState = States.Markered;
                    spriteRenderer.sprite = MarkeredSprite;
                    OnCellFlagged.Invoke(false);
                    break;

                case States.BeingPressed:
                    currentState = States.BeingPressed;
                    spriteRenderer.sprite = PressedSprite;
                    break;
            }
        }
    }
    private byte mines = 255;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = HiddenSprite;

        ParentField.OnFieldGenerated.AddListener(InitializeMinesVar);
    }

    private void InitializeMinesVar()
    {
        mines = ParentField.GetMinesAround(Position);
    }

    public void Dig(bool last = false)
    {
        if (CurrentState == States.Flagged || CurrentState == States.Markered) return;

        if (CurrentState == States.Shown)
        {
            if (!last && ParentField.GetFlagsAround(Position) == mines)
            {
                ParentField.DigAround(Position);
            }
            ParentField.SetSpriteToPressedAround(Position, false);
        }
        else
        {
            CurrentState = States.Shown;
            if (ParentField.IsMine(Position))
            {
                GameManager.instance.GameOver();
            }

            else if (mines == 0)
            {
                ParentField.DigAround(Position);
            }
        }
    }

    public void OnLeftMouseDown()
    {
        if (CurrentState == States.Shown)
            ParentField.SetSpriteToPressedAround(Position, true);
        else if (CurrentState == States.Hidden)
            CurrentState = States.BeingPressed;
    }

    public void OnRightMouseUp()
    {
        if (CurrentState == States.Shown) return;

        if (CurrentState == States.Hidden)
            CurrentState = States.Flagged;
        else if (CurrentState == States.Flagged)
            CurrentState = States.Markered;
        else
            CurrentState = States.Hidden;
    }

    //public void ResetState()
    //{
    //    CurrentState = States.Hidden;
    //    mines = 255;
    //}
}
